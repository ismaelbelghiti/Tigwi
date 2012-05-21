// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListRepository.cs" company="ENS Paris">
//   BSD
// </copyright>
// <summary>
//   An implementation of the <see cref="IListRepository" /> interface for Azure Storage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tigwi.UI.Models.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Tigwi.Storage.Library;

    /// <summary>
    /// An implementation of the <see cref="IListRepository" /> interface for Azure Storage.
    /// </summary>
    public class ListRepository : StorageEntityRepository<StorageListModel>, IListRepository
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListRepository"/> class. 
        /// </summary>
        /// <param name="storageContext">
        /// The storage context.
        /// </param>
        public ListRepository(StorageContext storageContext)
            : base(storageContext)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a new list associated with the given account.
        /// </summary>
        /// <param name="account">
        /// The account to be the list's owner.
        /// </param>
        /// <param name="name">
        /// The list's name.
        /// </param>
        /// <param name="description">
        /// The list's description.
        /// </param>
        /// <param name="isPrivate">
        /// A boolean indicating whether the list is private or public.
        /// </param>
        /// <returns>
        /// An <see cref="IListModel"/> representing the newly created list.
        /// </returns>
        /// <exception cref="AccountNotFoundException">
        /// When the account does not exist.
        /// </exception>
        public IListModel Create(IAccountModel account, string name, string description, bool isPrivate)
        {
            try
            {
                var id = this.Storage.List.Create(account.Id, name, description, isPrivate);
                return this.Find(id);
            }
            catch (AccountNotFound ex)
            {
                throw new AccountNotFoundException(name, ex);
            }
        }

        /// <summary>
        /// Delete a given list.
        /// </summary>
        /// <param name="list">
        /// The list to delete.
        /// </param>
        public void Delete(IListModel list)
        {
            // TODO: fixme
            this.Storage.List.Delete(list.Id);
            this.EntitiesMap.Remove(list.Id);
        }

        /// <summary>
        /// Find a list by its ID.
        /// </summary>
        /// <param name="listId">
        /// The list id.
        /// </param>
        /// <returns>
        /// A lazy <see cref="IListModel" /> representing the list.
        /// </returns>
        public IListModel Find(Guid listId)
        {
            // Simply a type cast
            return this.InternalFind(listId);
        }

        public IListModelCollection Find(ICollection<Guid> listsId)
        {
            return new StorageListModelCollection(this.StorageContext, listsId);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find a list by its ID and returns it with its true type, <see cref="StorageListModel" />.
        /// </summary>
        /// <param name="listId">
        /// The list id.
        /// </param>
        /// <returns>
        /// A lazy <see cref="StorageListModel" /> representing the list.
        /// </returns>
        internal StorageListModel InternalFind(Guid listId)
        {
            StorageListModel list;
            
            // Try to hit the cache
            if (!this.EntitiesMap.TryGetValue(listId, out list))
            {
                list = new StorageListModel(this.Storage, this.StorageContext, listId);
                this.EntitiesMap.Add(listId, list);
            }

            return list;
        }

        /// <summary>
        /// Commit the changes.
        /// </summary>
        internal bool SaveChanges()
        {
            // Fold, yay!
            return this.EntitiesMap.Aggregate(true, (current, list) => current & list.Value.Save());
        }

        #endregion
    }
}