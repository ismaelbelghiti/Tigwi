namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    #endregion

    public abstract class StorageEntityCollection<TParent, TModel, TInterface> : ICollection<TInterface>
        where TModel : StorageEntityModel, TInterface
    {
        #region Constants and Fields

        private readonly IDictionary<TModel, bool> collectionAdded;

        private readonly IDictionary<TModel, bool> collectionRemoved;

        private readonly ICollection<TModel> internalCollection;

        private readonly IStorage storage;

        private readonly IStorageContext storageContext;

        #endregion

        #region Constructors and Destructors

        protected StorageEntityCollection(
            IStorage storage, IStorageContext storageContext, TParent parent, Func<ICollection<Guid>> fetchIdCollection, Func<TInterface, Guid> getId)
        {
            this.FetchIdCollection = fetchIdCollection;
            this.storage = storage;
            this.storageContext = storageContext;
            this.Parent = parent;
            this.internalCollection = new HashSet<TModel>();
            this.collectionAdded = new Dictionary<TModel, bool>();
            this.collectionRemoved = new Dictionary<TModel, bool>();
            this.GetId = getId;
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get
            {
                this.CleanInternalCollection();
                return this.InternalCollection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.internalCollection.IsReadOnly;
            }
        }

        #endregion

        #region Properties

        protected ICollection<TModel> CachedCollection
        {
            get
            {
                return this.internalCollection;
            }
        }

        protected IDictionary<TModel, bool> CollectionAdded
        {
            get
            {
                return this.collectionAdded;
            }
        }

        protected IDictionary<TModel, bool> CollectionRemoved
        {
            get
            {
                return this.collectionRemoved;
            }
        }

        protected Func<ICollection<Guid>> FetchIdCollection { get; private set; }

        protected Func<Guid, TModel> GetModel { get; set; }

        protected Func<TInterface, Guid> GetId { get; set; }

        protected ICollection<TModel> InternalCollection
        {
            get
            {
                if (!this.InternalCollectionFetched)
                {
                    this.UpdateInternalCollection();
                    this.PatchInternalCollection();

                    this.InternalCollectionFetched = true;
                }

                this.CleanInternalCollection();

                return this.internalCollection;
            }
        }

        protected bool InternalCollectionFetched { get; set; }

        protected TParent Parent { get; private set; }

        protected IStorage Storage
        {
            get
            {
                return this.storage;
            }
        }

        protected IStorageContext StorageContext
        {
            get
            {
                return this.storageContext;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Add(TInterface item)
        {
            this.Add(item as TModel ?? this.GetModel(this.GetId(item)), responsible: true);
        }

        public void Clear()
        {
            foreach (var item in this.InternalCollection)
            {
                this.CollectionRemoved.Add(item, true);
                this.ReverseRemove(item);
            }

            this.InternalCollection.Clear();
            this.CollectionAdded.Clear();
        }

        public bool Contains(TInterface item)
        {
            return this.InternalCollection.Contains(item as TModel ?? this.GetModel(this.GetId(item)));
        }

        public void CopyTo(TInterface[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Length + arrayIndex > this.InternalCollection.Count)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            var i = arrayIndex;
            foreach (var model in this.InternalCollection)
            {
                array[i++] = model;
            }
        }

        public IEnumerator<TInterface> GetEnumerator()
        {
            var toRemove = new HashSet<TModel>();
            foreach (var entity in this.InternalCollection)
            {
                if (entity.Deleted)
                {
                    toRemove.Add(entity);
                }
                else
                {
                    yield return entity;

                }
            }

            foreach (var entity in toRemove)
            {
                this.internalCollection.Remove(entity);
            }
        }

        public bool Remove(TInterface item)
        {
            return this.Remove(item as TModel ?? this.GetModel(this.GetId(item)), true);
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Methods

        internal void CacheAdd(TModel item)
        {
            this.Add(item, responsible: false);
        }

        internal bool CacheRemove(TModel item)
        {
            return this.Remove(item, false);
        }

        internal abstract void Save();

        protected void Add(TModel item, bool responsible)
        {
            this.CollectionRemoved.Remove(item);
            this.CollectionAdded.Add(item, responsible);
            this.internalCollection.Add(item);
            if (responsible)
            {
                this.ReverseAdd(item);
            }
        }

        protected void CleanInternalCollection()
        {
            var toRemove = new HashSet<TModel>();

            foreach (var entity in this.internalCollection.Where(entity => entity.Deleted))
            {
                toRemove.Add(entity);
            }

            foreach (var entity in toRemove)
            {
                this.internalCollection.Remove(entity);
            }
        }

        protected void PatchInternalCollection()
        {
            foreach (var item in this.CollectionAdded.Where(item => !item.Key.Deleted).Select(item => item.Key))
            {
                this.internalCollection.Add(item);
            }

            foreach (var item in this.CollectionRemoved.Select(item => item.Key))
            {
                this.internalCollection.Remove(item);
            }
        }

        protected bool Remove(TModel item, bool responsible)
        {
            var result = this.internalCollection.Remove(item);

            this.CollectionRemoved.Add(item, responsible);
            this.CollectionAdded.Remove(item);

            if (responsible)
            {
                this.ReverseRemove(item);
            }

            return result;
        }

        protected abstract void ReverseAdd(TModel item);

        protected abstract void ReverseRemove(TModel item);

        protected void UpdateInternalCollection()
        {
            foreach (var guid in this.FetchIdCollection())
            {
                var model = this.GetModel(guid);
                if (!model.Deleted)
                {
                    this.internalCollection.Add(model);
                }
            }
        }

        #endregion
    }
}