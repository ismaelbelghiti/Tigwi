#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
namespace Tigwi.UI.Models
{
    using System;
    using System.Collections.Generic;

    using Tigwi.UI.Models.Storage;

    /// <summary>
    /// The interface specification for a list repository.
    /// </summary>
    public interface IListRepository
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a new list associated with the given account.
        /// </summary>
        /// <param name="account">
        /// The new list's initial owner.
        /// </param>
        /// <param name="name">
        /// The new list's name.
        /// </param>
        /// <param name="description">
        /// The new list's description.
        /// </param>
        /// <param name="isPrivate">
        /// Whether the new list shall be private or not.
        /// </param>
        /// <exception cref="AccountNotFoundException">When the account no longer exists.</exception>
        /// <returns>
        /// </returns>
        IListModel Create(IAccountModel account, string name, string description, bool isPrivate);

        /// <summary>
        /// Deletes the given list.
        /// </summary>
        /// <param name="list">
        /// The list to delete.
        /// </param>
        void Delete(IListModel list);

        /// <summary>
        /// Finds the <see cref="IListModel"/> with the given ID.
        /// </summary>
        /// <param name="listId">
        /// The id of the list to find.
        /// </param>
        /// <returns>
        /// </returns>
        IListModel Find(Guid listId);

        /// <summary>
        /// Finds the <see cref="IListModelCollection"/> corresponding to the given set of IDs.
        /// </summary>
        /// <param name="listsId">The set of IDs to find</param>
        /// <returns></returns>
        IListModelCollection Find(ICollection<Guid> listsId);

        #endregion
    }
}