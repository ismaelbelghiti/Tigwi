// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   The collection adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The collection adapter.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class CollectionAdapter<T> : ICollection<T>
    {
        #region Constants and Fields

        /// <summary>
        /// The collection added.
        /// </summary>
        private readonly ICollection<T> collectionAdded;

        /// <summary>
        /// The collection removed.
        /// </summary>
        private readonly ICollection<T> collectionRemoved;

        /// <summary>
        /// The internal collection.
        /// </summary>
        private readonly ICollection<T> internalCollection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionAdapter{T}"/> class. 
        /// </summary>
        protected CollectionAdapter()
        {
            this.InternalCollectionFetched = false;
            this.internalCollection = new HashSet<T>();
            this.collectionAdded = new HashSet<T>();
            this.collectionRemoved = new HashSet<T>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.InternalCollection.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsReadOnly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.InternalCollection.IsReadOnly;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets CollectionAdded.
        /// </summary>
        protected ICollection<T> CollectionAdded
        {
            get
            {
                return this.collectionAdded;
            }
        }

        /// <summary>
        /// Gets CollectionRemoved.
        /// </summary>
        protected ICollection<T> CollectionRemoved
        {
            get
            {
                return this.collectionRemoved;
            }
        }

        /// <summary>
        /// Gets InternalCollection.
        /// </summary>
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

        protected void UpdateInternalCollection()
        {
            foreach (var guid in this.FetchIdCollection())
            {
                this.internalCollection.Add(this.GetModel(guid));
            }
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether InternalCollectionFetched.
        /// </summary>
        protected bool InternalCollectionFetched { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(T item)
        {
            this.CollectionRemoved.Remove(item);
            this.CollectionAdded.Add(item);
            this.internalCollection.Add(item);
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            foreach (T item in this.InternalCollection)
            {
                this.CollectionRemoved.Add(item);
            }

            this.InternalCollection.Clear();
            this.CollectionAdded.Clear();
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The contains.
        /// </returns>
        public bool Contains(T item)
        {
            return this.InternalCollection.Contains(item);
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// The array index.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InternalCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.InternalCollection.GetEnumerator();
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The remove.
        /// </returns>
        public bool Remove(T item)
        {
            bool result = this.internalCollection.Remove(item);

            // Mark as removed
            this.CollectionRemoved.Add(item);
            this.CollectionAdded.Remove(item);

            return result;
        }

        /// <summary>
        /// The save.
        /// </summary>
        public abstract void Save();

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InternalCollection.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        /// TODO: Switch to factory.
        protected abstract T GetModel(Guid id);

        /// <summary>
        /// The fetch id collection.
        /// </summary>
        /// <returns>
        /// </returns>
        protected abstract ICollection<Guid> FetchIdCollection();

        #endregion
    }
}