using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IListStorage
    {
        // TODO : specify the exceptions that each methode can throw
        IListInfo GetInfo(int listId);
        void SetInfo(int listId, string name, string description, bool isPrivate);
        int GetOwner(int listId);

        int GetPersonalList(int accountId);

        int Create(int ownerId, string name, string description, bool isPrivate);
        void Delete(int id);

        void Follow(int listId, int accountId);
        void Unfollow(int listId, int accountId);

        HashSet<int> GetAccounts(int listId);
        void Add(int listId, int accountId);
        void Remove(int listId, int accountId);
        
        HashSet<int> GetAccountOwnedLists(int accountId, bool withPrivate);
        HashSet<int> GetAccountFollowedLists(int accountId, bool withPrivate);
        HashSet<int> GetFollowingLists(int accountId);
    }
}
