using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tigwi.Storage.Library
{
    public interface IListStorage
    {
        /// <summary>
        /// Get the infos about a list
        /// </summary>
        /// <exception cref="ListNotFound">if no list has this ID</exception>
        IListInfo GetInfo(Guid listId);

        /// <summary>
        /// Set the infos about a list
        /// </summary>
        /// <exception cref="ListNotFound">If no list has this ID</exception>
        /// <exception cref="IsPersonnalList">If you are trying to modify the list that correspond to the messages from a single user</exception>
        void SetInfo(Guid listId, string name, string description, bool isPrivate);

        /// <summary>
        /// Get the owner of a list
        /// </summary>
        /// <exception cref="ListNotFound"> If no list has this ID</exception>
        Guid GetOwner(Guid listId);

        /// <summary>
        /// Get the list following only the given account
        /// </summary>
        /// <exception cref="AccountNotFound">If no list has this ID</exception>
        Guid GetPersonalList(Guid accountId);

        /// <summary>
        /// Create a list
        /// Multiple lists can have the same name
        /// </summary>
        /// <exception cref="AccountNotFound">If no account has this Id</exception>
        Guid Create(Guid ownerId, string name, string description, bool isPrivate);

        /// <summary>
        /// Delete a list
        /// </summary>
        /// <exception cref="IsPersonnalList">If you are trying to delete the list that correspond to the messages from a single user</exception>
        void Delete(Guid id);

        /// <summary>
        /// The account accountId follow listId
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has this ID</exception>
        /// <exception cref="ListNotFound">if no list has this ID</exception>
        void Follow(Guid listId, Guid accountId);

        /// <summary>
        /// The account accountId does not follow listId anymore
        /// </summary>
        /// <exception cref="AccountNotFound">if no account has this ID</exception>
        /// <exception cref="ListNotFound">no list has this ID</exception>
        /// <exception cref="AccountIsOwner">you must follow the lists you own</exception>
        void Unfollow(Guid listId, Guid accountId);

        /// <summary>
        /// Get the accounts that are in the given list
        /// </summary>
        /// <exception cref="ListNotFound">No list has this ID</exception>
        HashSet<Guid> GetAccounts(Guid listId);

        /// <summary>
        /// Add an account into the given list
        /// </summary>
        /// <exception cref="ListNotFound">no list has this ID</exception>
        /// <exception cref="AccountNotFound">no account has this ID</exception>
        void Add(Guid listId, Guid accountId);

        /// <summary>
        /// Remove an account from the given list
        /// </summary>
        void Remove(Guid listId, Guid accountId);

        /// <summary>
        /// Get the lits created by the given account
        /// </summary>
        /// <exception cref="AccountNotFound">no account has this Id</exception>
        HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate);

        /// <summary>
        /// Get the lists Followed by the given account
        /// </summary>
        /// <exception cref="AccountNotFound">no account has this ID</exception>
        HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate);

        /// <summary>
        /// Get the lists following the given account
        /// </summary>
        /// <exception cref="AccountNotFound">no account has this ID</exception>
        HashSet<Guid> GetFollowingLists(Guid accountId);

        /// <summary>
        /// Get the accounts following publicly the given list
        /// </summary>
        /// <exception cref="ListNotFound">No list has this ID</exception>
        HashSet<Guid> GetFollowingAccounts(Guid listId);
    }
}
