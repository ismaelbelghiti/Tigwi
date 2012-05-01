namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using StorageLibrary;

    #endregion

    public abstract class StorageEntityCollection<TParent, T> : ICollection<T>
        where T : StorageEntityModel
    {
        #region Constants and Fields

        private readonly IDictionary<T, bool> collectionAdded;

        private readonly IDictionary<T, bool> collectionRemoved;

        private readonly ICollection<T> internalCollection;

        private readonly IStorage storage;

        private readonly IStorageContext storageContext;

        #endregion

        #region Constructors and Destructors

        protected StorageEntityCollection(
            IStorage storage, IStorageContext storageContext, TParent parent, Func<ICollection<Guid>> fetchIdCollection)
        {
            this.FetchIdCollection = fetchIdCollection;
            this.storage = storage;
            this.storageContext = storageContext;
            this.Parent = parent;
            this.internalCollection = new HashSet<T>();
            this.collectionAdded = new Dictionary<T, bool>();
            this.collectionRemoved = new Dictionary<T, bool>();
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

        protected ICollection<T> CachedCollection
        {
            get
            {
                return this.internalCollection;
            }
        }

        protected IDictionary<T, bool> CollectionAdded
        {
            get
            {
                return this.collectionAdded;
            }
        }

        protected IDictionary<T, bool> CollectionRemoved
        {
            get
            {
                return this.collectionRemoved;
            }
        }

        protected Func<ICollection<Guid>> FetchIdCollection { get; private set; }

        protected Func<Guid, T> GetModel { get; set; }

        protected ICollection<T> InternalCollection
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

        protected void CleanInternalCollection()
        {
            var toRemove = new HashSet<T>();

            foreach (var entity in this.internalCollection.Where(entity => entity.Deleted))
            {
                toRemove.Add(entity);
            }

            foreach (var entity in toRemove)
            {
                this.internalCollection.Remove(entity);
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

        public void Add(T item)
        {
            this.Add(item, responsible: true);
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

        public bool Contains(T item)
        {
            return this.InternalCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InternalCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var toRemove = new HashSet<T>();
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

        public bool Remove(T item)
        {
            return this.Remove(item, true);
        }

        internal abstract void Save();

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Methods

        internal void CacheAdd(T item)
        {
            this.Add(item, responsible: false);
        }

        internal bool CacheRemove(T item)
        {
            return this.Remove(item, false);
        }

        protected void Add(T item, bool responsible)
        {
            this.CollectionRemoved.Remove(item);
            this.CollectionAdded.Add(item, responsible);
            this.internalCollection.Add(item);
            if (responsible)
            {
                this.ReverseAdd(item);
            }
        }

        protected void PatchInternalCollection()
        {
            foreach (var item in this.CollectionAdded.Select(item => item.Key))
            {
                this.internalCollection.Add(item);
            }

            foreach (var item in this.CollectionRemoved.Select(item => item.Key))
            {
                this.internalCollection.Remove(item);
            }
        }

        protected bool Remove(T item, bool responsible)
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

        protected abstract void ReverseAdd(T item);

        protected abstract void ReverseRemove(T item);

        protected void UpdateInternalCollection()
        {
            foreach (var guid in this.FetchIdCollection())
            {
                this.internalCollection.Add(this.GetModel(guid));
            }
        }

        #endregion
    }
}