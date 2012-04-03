using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IListStorage
    {
        /// <summary>
        /// Get the infos about a list
        /// Can throw : ListNotFound
        /// </summary>
        IListInfo GetInfo(Guid listId);
        /// <summary>
        /// Set the infos about a list
        /// Can throw : ListNotFound, IsPersonalList
        /// </summary>
        void SetInfo(Guid listId, string name, string description, bool isPrivate);
        /// <summary>
        /// Get the owner of a list
        /// Can throw : ListNotFoud
        /// </summary>
        Guid GetOwner(Guid listId);

        /// <summary>
        /// Get the list following only the given account
        /// Can throw : ListNotFound
        /// </summary>
        Guid GetPersonalList(Guid accountId);

        /// <summary>
        /// Create a list
        /// Can throw : AccountNotFound
        /// </summary>
        Guid Create(Guid ownerId, string name, string description, bool isPrivate);
        /// <summary>
        /// Delete a list
        /// Can throw : IsPersonalList
        /// </summary>
        void Delete(Guid id);

        /// <summary>
        /// The account accountId follow listId
        /// Can throw : AccountNotFound, ListNotFound
        /// </summary>
        void Follow(Guid listId, Guid accountId);
        /// <summary>
        /// The account accountId does not follow listId anymore
        /// Can throw : AccountNotFound, ListNotFound
        /// </summary>
        void Unfollow(Guid listId, Guid accountId);

        /// <summary>
        /// Get the accounts that are in the given list
        /// Can throw : ListNotFound
        /// </summary>
        HashSet<Guid> GetAccounts(Guid listId);
        /// <summary>
        /// Add an account into the given list
        /// Can throw : ListNotFound, AccountNotFound
        /// </summary>
        void Add(Guid listId, Guid accountId);
        /// <summary>
        /// Remove an account from the given list
        /// Can throw : ListNotFound
        /// </summary>
        void Remove(Guid listId, Guid accountId);

        /// <summary>
        /// Get the lits created by the given account
        /// Can throw : AccountNotFound
        /// </summary>
        HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate);
        /// <summary>
        /// Get the lists Followed by the given account
        /// Can throw : AccountNotFound
        /// </summary>
        HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate);
        /// <summary>
        /// Get the lists following the given account
        /// Can throw : AccountNotFound
        /// </summary>
        HashSet<Guid> GetFollowingLists(Guid accountId);
        /// <summary>
        /// Get the accounts following the given list
        /// Can throw : ListNotFound
        /// </summary>
        HashSet<Guid> GetFollowingAccounts(Guid listId);
    }
}
