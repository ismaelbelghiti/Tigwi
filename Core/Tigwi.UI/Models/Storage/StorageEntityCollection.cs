namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    internal class StorageEntityCollection<TModel, TInterface> : ICollection<TInterface>
        where TModel : StorageEntityModel, TInterface
    {
        #region Constants and Fields

        private readonly IDictionary<TModel, bool> collectionAdded;

        private readonly IDictionary<TModel, bool> collectionRemoved;

        private readonly IDictionary<Guid, TModel> internalCollection;

        private readonly StorageContext storageContext;

        #endregion

        #region Constructors and Destructors

        internal StorageEntityCollection(StorageContext storageContext)
        {
            this.storageContext = storageContext;
            this.internalCollection = new Dictionary<Guid, TModel>();
            this.collectionAdded = new Dictionary<TModel, bool>();
            this.collectionRemoved = new Dictionary<TModel, bool>();
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

        internal Func<ICollection<Guid>> FetchIdCollection { get; set; }

        internal Func<TInterface, Guid> GetId { get; set; }

        internal Func<Guid, TModel> GetModel { get; set; }

        internal Action<TModel> ReverseAdd { get; set; }

        internal Action<TModel> ReverseRemove { get; set; }

        internal Action<TModel> SaveAdd { get; set; }

        internal Action<TModel> SaveRemove { get; set; }

        protected IDictionary<Guid, TModel> CachedCollection
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

        protected IDictionary<Guid, TModel> InternalCollection
        {
            get
            {
                if (!this.InternalCollectionFetched)
                {
                    this.FetchInternalCollection();
                    this.PatchInternalCollection();

                    this.InternalCollectionFetched = true;
                }

                this.CleanInternalCollection();

                return this.internalCollection;
            }
        }

        protected bool InternalCollectionFetched { get; set; }

        protected IStorage Storage
        {
            get
            {
                return this.storageContext.StorageObj;
            }
        }

        protected StorageContext StorageContext
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
            foreach (var item in this.InternalCollection.Values)
            {
                this.CollectionRemoved.Add(item, true);
                this.ReverseRemove(item);
            }

            this.InternalCollection.Clear();
            this.CollectionAdded.Clear();
        }

        public bool Contains(TInterface item)
        {
            return this.InternalCollection.ContainsKey(this.GetId(item));
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
            foreach (var model in this.InternalCollection.Values)
            {
                array[i++] = model;
            }
        }

        public IEnumerator<TInterface> GetEnumerator()
        {
            var toRemove = new HashSet<Guid>();
            foreach (var entity in this.InternalCollection.Values)
            {
                if (entity.Deleted)
                {
                    toRemove.Add(entity.Id);
                }
                else
                {
                    yield return entity;
                }
            }

            foreach (var id in toRemove)
            {
                this.internalCollection.Remove(id);
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
            return this.Remove(item, responsible: false);
        }

        internal void Save()
        {
            foreach (var item in this.CollectionAdded.Where(item => item.Value).Select(item => item.Key))
            {
                this.SaveAdd(item);
            }

            foreach (var item in this.CollectionRemoved.Where(item => item.Value).Select(item => item.Key))
            {
                this.SaveRemove(item);
            }

            this.CollectionAdded.Clear();
            this.CollectionRemoved.Clear();
        }

        protected void Add(TModel item, bool responsible)
        {
            this.CollectionRemoved.Remove(item);
            this.CollectionAdded.Add(item, responsible);
            this.internalCollection.Add(item.Id, item);

            if (responsible)
            {
                this.ReverseAdd(item);
            }
        }

        protected void CleanInternalCollection()
        {
            var toRemove = new HashSet<Guid>();

            foreach (var id in this.internalCollection.Where(entity => entity.Value.Deleted).Select(entity => entity.Key))
            {
                toRemove.Add(id);
            }

            foreach (var id in toRemove)
            {
                this.internalCollection.Remove(id);
            }
        }

        protected void PatchInternalCollection()
        {
            foreach (var item in this.CollectionAdded.Where(item => !item.Key.Deleted).Select(item => item.Key))
            {
                this.internalCollection.Add(item.Id, item);
            }

            foreach (var id in this.CollectionRemoved.Select(item => item.Key.Id))
            {
                this.internalCollection.Remove(id);
            }
        }

        protected bool Remove(TModel item, bool responsible)
        {
            var result = this.internalCollection.Remove(item.Id);

            this.CollectionRemoved.Add(item, responsible);
            this.CollectionAdded.Remove(item);

            if (responsible)
            {
                this.ReverseRemove(item);
            }

            return result;
        }

        protected void FetchInternalCollection()
        {
            this.internalCollection.Clear();
            foreach (var model in this.FetchIdCollection().Select(guid => this.GetModel(guid)).Where(model => !model.Deleted))
            {
                this.internalCollection.Add(model.Id, model);
            }
        }

        #endregion
    }
}