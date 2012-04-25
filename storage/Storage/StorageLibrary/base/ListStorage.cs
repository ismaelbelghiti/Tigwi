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
            StrgBlob<IListInfo> blob = new StrgBlob<IListInfo>(connexion.listContainer, Path.L_INFO + listId);
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
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.listContainer, Path.L_OWNER + listId);
            return blob.GetIfExists(new ListNotFound());
        }

        // NYI
        public Guid GetPersonalList(Guid accountId)
        {
            throw new NotImplementedException();

            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.listContainer, Path.L_PERSO + accountId);
            return blob.GetIfExists(new ListNotFound());
        }

        public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
        {
            // implemntation partielle

            // TODO : verifier que l'utilisateur existe
            // TODO : implementer les autres structues de données

            // Create the data :
            Guid id = Guid.NewGuid();
            ListInfo info = new ListInfo(name, description, isPrivate, false);
            HashSet<Guid> followingAccounts = new HashSet<Guid>();
            followingAccounts.Add(ownerId);

            // Creation des blobs
            StrgBlob<ListInfo> bInfo = new StrgBlob<ListInfo>(connexion.listContainer, Path.L_INFO + id);
            StrgBlob<Guid> bOwner = new StrgBlob<Guid>(connexion.listContainer, Path.L_OWNER + id);
            HashSetBlob<Guid> bOwned = new HashSetBlob<Guid>(connexion.listContainer, (isPrivate ? Path.L_OWNEDLISTS_PRIVATE : Path.L_OWNEDLISTS_PUBLIC) + ownerId);
            StrgBlob<HashSet<Guid>> bFollowingAccounts = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWINGACCOUNTS + id);
            StrgBlob<HashSet<Guid>> bFollowedAccounts = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + id + Path.L_FOLLOWEDACC_DATA);

            // store the data
            bInfo.Set(info);
            bOwner.Set(ownerId);
            bFollowingAccounts.Set(followingAccounts);
            bFollowedAccounts.Set(new HashSet<Guid>());
            Mutex.Init(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + id + Path.L_FOLLOWEDACC_LOCK);

            // add the lists to owned lists and check that the user exists. if he doesn't, delete the data stored
            if (!bOwned.Add(id))
            {
                bInfo.Delete();
                bOwner.Delete();
                bFollowingAccounts.Delete();
                bFollowedAccounts.Delete();
                Mutex.Delete(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + id + Path.L_FOLLOWEDACC_LOCK);
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
            StrgBlob<HashSet<Guid>> bFollowedLists = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_DATA);

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
            StrgBlob<HashSet<Guid>> bFollowedLists = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_DATA);
            StrgBlob<Guid> bOwner = new StrgBlob<Guid>(connexion.listContainer, Path.L_OWNER + listId);

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
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_DATA);
            return blob.GetIfExists(new ListNotFound());
        }

        public HashSet<Guid> GetFollowingLists(Guid accountId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDBY + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public void Add(Guid listId, Guid accountId)
        {
            StrgBlob<HashSet<Guid>> bFollowedAcounts = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_DATA);
            HashSetBlob<Guid> bFollowingLists = new HashSetBlob<Guid>(connexion.listContainer, Path.L_FOLLOWEDBY + accountId);
            StrgBlob<IListInfo> bListInfo = new StrgBlob<IListInfo>(connexion.listContainer, Path.L_INFO + listId);

            using (new Mutex(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_LOCK, new ListNotFound()))
            {
                // check if the list is private or not
                if ((!bListInfo.Get().IsPrivate && !bFollowingLists.Add(listId)) || !bFollowedAcounts.Exists)
                    throw new AccountNotFound();

                HashSet<Guid> followedAccounts = bFollowedAcounts.Get();
                followedAccounts.Add(accountId);
                bFollowedAcounts.Set(followedAccounts);
            }

        }

        public void Remove(Guid listId, Guid accountId)
        {
            // We don't check wether the list is private or not because it would be much more complicated and slower
            // it is much easier to remove the list form FollowingLists even if she doesn't belong to this set
            StrgBlob<HashSet<Guid>> bFollowedAcounts = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_DATA);

            try
            {
                using (new Mutex(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + listId + Path.L_FOLLOWEDACC_LOCK, new AccountNotFound()))
                {
                    HashSetBlob<Guid> bFollowingLists = new HashSetBlob<Guid>(connexion.listContainer, Path.L_FOLLOWEDBY + accountId);
                    bFollowingLists.Remove(listId);

                    HashSet<Guid> followedAccounts = bFollowedAcounts.Get();
                    followedAccounts.Remove(accountId);
                    bFollowedAcounts.Set(followedAccounts);
                }
            }
            catch (AccountNotFound) { }
        }

        public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
        {
            StrgBlob<HashSet<Guid>> bPublic = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PUBLIC + accountId);
            HashSet<Guid> lists = bPublic.GetIfExists(new AccountNotFound());
            if(withPrivate)
            {
                StrgBlob<HashSet<Guid>> bPrivate = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PRIVATE + accountId);
                lists = (HashSet<Guid>) lists.Concat(bPrivate.GetIfExists(new AccountNotFound()));
            }

            return lists;
        }

        public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
        {
            StrgBlob<HashSet<Guid>> bPublic = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PUBLIC + accountId);
            StrgBlob<HashSet<Guid>> bFollowed = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDLISTS + accountId + Path.L_FOLLOWEDLISTS_DATA);
            HashSet<Guid> lists = bPublic.GetIfExists(new AccountNotFound());
            lists.UnionWith(bFollowed.GetIfExists(new AccountNotFound()));

            if (withPrivate)
            {
                StrgBlob<HashSet<Guid>> bPrivate = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PRIVATE + accountId);
                lists = (HashSet<Guid>)lists.Concat(bPrivate.GetIfExists(new AccountNotFound()));
            }

            return lists;
        }

        public HashSet<Guid> GetFollowingAccounts(Guid listId)
        {
            StrgBlob<HashSet<Guid>> bFollowingAccounts = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWINGACCOUNTS + listId);
            return bFollowingAccounts.GetIfExists(new ListNotFound());
        }
    }
}
