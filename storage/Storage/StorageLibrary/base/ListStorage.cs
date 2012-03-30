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
            StrgBlob<IListInfo> blob = new StrgBlob<IListInfo>(connexion.listContainer, "info/" + listId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }

        public void SetInfo(int listId, string name, string description, bool isPrivate)
        {
            throw new NotImplementedException();
        }

        public int GetOwner(int listId)
        {
            StrgBlob<int> blob = new StrgBlob<int>(connexion.listContainer, "owner/" + listId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }

        public int GetPersonalList(int accountId)
        {
            StrgBlob<int> blob = new StrgBlob<int>(connexion.listContainer, "personallist/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
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
            StrgBlob<HashSet<int>> blob = new StrgBlob<HashSet<int>>(connexion.listContainer, "accounts/" + listId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
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
            StrgBlob<HashSet<int>> blob = new StrgBlob<HashSet<int>>(connexion.listContainer, "followinglists/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }
    }
}
