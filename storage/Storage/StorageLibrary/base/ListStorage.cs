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
        public IListInfo GetInfo(Guid listId)
        {
            StrgBlob<IListInfo> blob = new StrgBlob<IListInfo>(connexion.listContainer, "info/" + listId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }

        public void SetInfo(Guid listId, string name, string description, bool isPrivate)
        {
            throw new NotImplementedException();
        }

        public Guid GetOwner(Guid listId)
        {
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.listContainer, "owner/" + listId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }

        public Guid GetPersonalList(Guid accountId)
        {
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.listContainer, "personallist/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }

        public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Follow(Guid listId, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void Unfollow(Guid listId, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetAccounts(Guid listId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.listContainer, "accounts/" + listId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }

        public void Add(Guid listId, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid listId, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
        {
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetFollowingLists(Guid accountId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.listContainer, "followinglists/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.ListNotFound));
        }

        public HashSet<Guid> GetFollowingAccounts(Guid listId)
        {
            throw new NotImplementedException();
        }
    }
}
