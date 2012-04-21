namespace Tigwi.UI.Models.Storage
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    public abstract class CollectionAdapter<T> : ICollection<T>
    {
        #region Constants and Fields

        private readonly ICollection<T> collectionAdded;

        private readonly ICollection<T> collectionRemoved;

        private readonly ICollection<T> internalCollection;

        #endregion

        #region Constructors and Destructors

        protected CollectionAdapter()
        {
            this.InternalCollectionFetched = false;
            this.internalCollection = new HashSet<T>();
            this.collectionAdded = new HashSet<T>();
            this.collectionRemoved = new HashSet<T>();
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get
            {
                return this.InternalCollection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.InternalCollection.IsReadOnly;
            }
        }

        #endregion

        #region Properties

        protected ICollection<T> CollectionAdded
        {
            get
            {
                return this.collectionAdded;
            }
        }

        protected ICollection<T> CollectionRemoved
        {
            get
            {
                return this.collectionRemoved;
            }
        }

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

                return this.internalCollection;
            }
        }

        protected bool InternalCollectionFetched { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Add(T item)
        {
            this.CollectionRemoved.Remove(item);
            this.CollectionAdded.Add(item);
            this.internalCollection.Add(item);
        }

        public void Clear()
        {
            foreach (var item in this.InternalCollection)
            {
                this.CollectionRemoved.Add(item);
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
            return this.InternalCollection.GetEnumerator();
        }

        public bool Remove(T item)
        {
            bool result = this.internalCollection.Remove(item);

            // Mark as removed
            this.CollectionRemoved.Add(item);
            this.CollectionAdded.Remove(item);

            return result;
        }

        public abstract void Save();

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InternalCollection.GetEnumerator();
        }

        #endregion

        #region Methods

        protected abstract ICollection<Guid> FetchIdCollection();

        protected abstract T GetModel(Guid id);

        protected void PatchInternalCollection()
        {
            foreach (var item in this.CollectionAdded)
            {
                this.internalCollection.Add(item);
            }

            foreach (var item in this.CollectionRemoved)
            {
                this.internalCollection.Remove(item);
            }
        }

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