using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary;
using StorageLibrary.Utilities;

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
            Blob<IListInfo> blob = new Blob<IListInfo>(connexion.listContainer, Path.L_INFO + listId);
            return blob.GetIfExists(new ListNotFound());
        }
        
        // NYI
        public void SetInfo(Guid listId, string name, string description, bool isPrivate)
        {
            // autorisation passage publique/privé
            // si passage publique privé : pseudo-suppression

            throw new NotImplementedException();
        }

        public Guid GetOwner(Guid listId)
        {
            Blob<Guid> blob = new Blob<Guid>(connexion.listContainer, Path.L_OWNER + listId);
            return blob.GetIfExists(new ListNotFound());
        }

        public Guid GetPersonalList(Guid accountId)
        {
            Blob<Guid> blob = new Blob<Guid>(connexion.listContainer, Path.L_PERSO + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
        {
            // Create the data :
            Guid id = Guid.NewGuid();
            ListInfo info = new ListInfo(name, description, isPrivate, false);
            HashSet<Guid> followingAccounts = new HashSet<Guid>();
            followingAccounts.Add(ownerId);

            // Creation of blobs in list container
            Blob<ListInfo> bInfo = new Blob<ListInfo>(connexion.listContainer, Path.L_INFO + id);
            Blob<Guid> bOwner = new Blob<Guid>(connexion.listContainer, Path.L_OWNER + id);
            HashSetBlob<Guid> bOwned = new HashSetBlob<Guid>(connexion.listContainer, (isPrivate ? Path.L_OWNEDLISTS_PRIVATE : Path.L_OWNEDLISTS_PUBLIC) + ownerId);
            Blob<HashSet<Guid>> bFollowingAccounts = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWINGACCOUNTS + id);
            Blob<HashSet<Guid>> bFollowedAccounts = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + id + Path.L_FOLLOWEDACC_DATA);

            MsgSetBlobPack bMessages = new MsgSetBlobPack(connexion.msgContainer, Path.M_LISTMESSAGES + id);

            // store the data
            bInfo.Set(info);
            bOwner.Set(ownerId);
            bFollowingAccounts.Set(followingAccounts);
            bFollowedAccounts.Set(new HashSet<Guid>());
            Mutex.Init(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + id + Path.L_FOLLOWEDACC_LOCK);

            bMessages.Init();

            // add the lists to owned lists and check that the user exists. if he doesn't, delete the data stored
            if (!bOwned.Add(id))
            {
                bInfo.Delete();
                bOwner.Delete();
                bFollowingAccounts.Delete();
                bFollowedAccounts.Delete();
                Mutex.Delete(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + id + Path.L_FOLLOWEDACC_LOCK);

                bMessages.Delete();

                throw new AccountNotFound();
            }

            return id;
        }

        // NYI
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Follow(Guid listId, Guid accountId)
        {
            HashSetBlob<Guid> bFollowingAccounts = new HashSetBlob<Guid>(connexion.listContainer, Path.L_FOLLOWINGACCOUNTS + listId);
            Blob<HashSet<Guid>> bFollowedLists = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_DATA);

            using(new Mutex(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_LOCK, new AccountNotFound()))
            {
                if (!bFollowingAccounts.Add(accountId))
                    throw new ListNotFound();

                HashSet<Guid> followedLists = bFollowedLists.Get();
                followedLists.Add(listId);
                bFollowedLists.Set(followedLists);
            }
        }

        public void Unfollow(Guid listId, Guid accountId)
        {
            HashSetBlob<Guid> bFollowingAccounts = new HashSetBlob<Guid>(connexion.listContainer, Path.L_FOLLOWINGACCOUNTS + listId);
            Blob<HashSet<Guid>> bFollowedLists = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_DATA);
            Blob<Guid> bOwner = new Blob<Guid>(connexion.listContainer, Path.L_OWNER + listId);

            // check that the account is not the owner
            if (bOwner.GetIfExists(new ListNotFound()) == accountId)
                throw new AccountIsOwner();

            using (new Mutex(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_LOCK, new AccountNotFound()))
            {
                bFollowingAccounts.Remove(accountId);
                HashSet<Guid> followedLists = bFollowedLists.GetIfExists(new ListNotFound());
                followedLists.Remove(listId);
                bFollowedLists.Set(followedLists);
            }

        }

        public HashSet<Guid> GetAccounts(Guid listId)
        {
            Blob<HashSet<Guid>> blob = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_DATA);
            return blob.GetIfExists(new ListNotFound());
        }

        public HashSet<Guid> GetFollowingLists(Guid accountId)
        {
            Blob<HashSet<Guid>> blob = new Blob<HashSet<Guid>>(connexion.listContainer, Path.LFollowedByPublic(accountId));
            return blob.GetIfExists(new AccountNotFound());
        }

        public void Add(Guid listId, Guid accountId)
        {
            Blob<HashSet<Guid>> bFollowedAcounts = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_DATA);
            HashSetBlob<Guid> bFollowedByPublic = new HashSetBlob<Guid>(connexion.listContainer, Path.LFollowedByPublic(accountId));
            HashSetBlob<Guid> bFollowedByAll = new HashSetBlob<Guid>(connexion.listContainer, Path.LFollowedByAll(accountId));
            Blob<IListInfo> bListInfo = new Blob<IListInfo>(connexion.listContainer, Path.L_INFO + listId);

            using (new Mutex(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_LOCK, new ListNotFound()))
            {
                if (!bFollowedByAll.Add(listId))
                    throw new AccountNotFound();

                // check if the list is private or not
                // the account exists because it would need to take the mutex to be deleted
                if (!bListInfo.Get().IsPrivate)
                    bFollowedByPublic.Add(listId);

                HashSet<Guid> followedAccounts = bFollowedAcounts.Get();
                followedAccounts.Add(accountId);
                bFollowedAcounts.Set(followedAccounts);
            }

        }

        public void Remove(Guid listId, Guid accountId)
        {
            // We don't check wether the list is private or not because it would be much more complicated and slower
            // it is much easier to remove the list form FollowingLists even if she doesn't belong to this set
            Blob<HashSet<Guid>> bFollowedAcounts = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_DATA);

            try
            {
                using (new Mutex(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_LOCK, new AccountNotFound()))
                {
                    HashSetBlob<Guid> bFollowedByPublic = new HashSetBlob<Guid>(connexion.listContainer, Path.LFollowedByPublic(accountId));
                    HashSetBlob<Guid> bFollowedByAll = new HashSetBlob<Guid>(connexion.listContainer, Path.LFollowedByAll(accountId));

                    HashSet<Guid> followedAccounts = bFollowedAcounts.Get();
                    followedAccounts.Remove(accountId);
                    bFollowedAcounts.Set(followedAccounts);

                    bFollowedByPublic.Remove(listId);
                    bFollowedByAll.Remove(listId);
                }
            }
            catch (AccountNotFound) { }
        }

        public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
        {
            Blob<HashSet<Guid>> bPublic = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PUBLIC + accountId);
            HashSet<Guid> lists = bPublic.GetIfExists(new AccountNotFound());
            if(withPrivate)
            {
                Blob<HashSet<Guid>> bPrivate = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PRIVATE + accountId);
                lists = (HashSet<Guid>) lists.Concat(bPrivate.GetIfExists(new AccountNotFound()));
            }

            return lists;
        }

        public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
        {
            Blob<HashSet<Guid>> bPublic = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PUBLIC + accountId);
            Blob<HashSet<Guid>> bFollowed = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_DATA);
            HashSet<Guid> lists = bPublic.GetIfExists(new AccountNotFound());
            lists.UnionWith(bFollowed.GetIfExists(new AccountNotFound()));

            if (withPrivate)
            {
                Blob<HashSet<Guid>> bPrivate = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PRIVATE + accountId);
                lists = (HashSet<Guid>)lists.Concat(bPrivate.GetIfExists(new AccountNotFound()));
            }

            return lists;
        }

        public HashSet<Guid> GetFollowingAccounts(Guid listId)
        {
            Blob<HashSet<Guid>> bFollowingAccounts = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWINGACCOUNTS + listId);
            return bFollowingAccounts.GetIfExists(new ListNotFound());
        }
    }
}
