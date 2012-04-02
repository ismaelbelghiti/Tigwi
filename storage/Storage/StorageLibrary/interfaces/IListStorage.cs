using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IListStorage
    {
        // TODO : specify the exceptions that each methode can throw
        IListInfo GetInfo(Guid listId);
        void SetInfo(Guid listId, string name, string description, bool isPrivate);
        Guid GetOwner(Guid listId);

        Guid GetPersonalList(Guid accountId);

        Guid Create(Guid ownerId, string name, string description, bool isPrivate);
        void Delete(Guid id);

        void Follow(Guid listId, Guid accountId);
        void Unfollow(Guid listId, Guid accountId);

        HashSet<Guid> GetAccounts(Guid listId);
        void Add(Guid listId, Guid accountId);
        void Remove(Guid listId, Guid accountId);
        
        HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate);
        HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate);
        HashSet<Guid> GetFollowingLists(Guid accountId);
        HashSet<Guid> GetFollowingAccounts(Guid listId);
    }
}
