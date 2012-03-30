using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageCommon;

namespace StorageLibrary
{
    public class ListStorage : IListStorage
    {
        StrgConnexion connexion;

        // Constuctor
        public ListStorage(StrgConnexion connexion)
        {
            this.connexion = connexion;
        }

        // Interface implementation
        public IListInfo GetInfo(int listId)
        {
            throw new NotImplementedException();
        }

        public void SetInfo(int listId, string name, string description, bool isPrivate)
        {
            throw new NotImplementedException();
        }

        public int GetOwner(int listId)
        {
            throw new NotImplementedException();
        }

        public int GetPersonalList(int accountId)
        {
            throw new NotImplementedException();
        }

        public int Create(int ownerId, string name, string description, bool isPrivate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Follow(int listId, int accountId)
        {
            throw new NotImplementedException();
        }

        public void Unfollow(int listId, int accountId)
        {
            throw new NotImplementedException();
        }

        public HashSet<int> GetAccounts(int listId)
        {
            throw new NotImplementedException();
        }

        public void Add(int listId, int accountId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int listId, int accountId)
        {
            throw new NotImplementedException();
        }

        public HashSet<int> GetAccountOwnedLists(int accountId, bool withPrivate)
        {
            throw new NotImplementedException();
        }

        public HashSet<int> GetAccountFollowedLists(int accountId, bool withPrivate)
        {
            throw new NotImplementedException();
        }

        public HashSet<int> GetFollowingLists(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
