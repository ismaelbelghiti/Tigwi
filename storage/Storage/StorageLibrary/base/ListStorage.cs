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
        StrgConnexion connexion; // TODO : to be removed
        BlobFactory blobFactory;

        // Constuctor
        public ListStorage(StrgConnexion connexion, BlobFactory blobFactory)
        {
            this.connexion = connexion;
            this.blobFactory = blobFactory;
        }

        // Interface implementation
        public IListInfo GetInfo(Guid listId)
        {
            return blobFactory.LInfo(listId).GetIfExists(new ListNotFound());
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
            return blobFactory.LOwner(listId).GetIfExists(new ListNotFound());
        }

        public Guid GetPersonalList(Guid accountId)
        {
            return blobFactory.LPersonnalList(accountId).GetIfExists(new AccountNotFound());
        }

        public Guid Create(Guid ownerId, string name, string description, bool isPrivate)
        {
            // Create the data :
            Guid listId = Guid.NewGuid();
            ListInfo info = new ListInfo(name, description, isPrivate, false);
            HashSet<Guid> followingAccounts = new HashSet<Guid>();
            followingAccounts.Add(ownerId);

            // Creation of blobs in list container
            Blob<IListInfo> bInfo = blobFactory.LInfo(listId);
            Blob<Guid> bOwner = blobFactory.LOwner(listId);
            HashSetBlob<Guid> bOwned = isPrivate ? blobFactory.LOwnedListsPrivate(ownerId) : blobFactory.LOwnedListsPublic(ownerId);
            Blob<HashSet<Guid>> bFollowingAccounts = blobFactory.LFollowingAccounts(listId);
            Blob<HashSet<Guid>> bFollowedAccounts = blobFactory.LFollowedAccountsData(listId);
            MsgSetBlobPack bMessages = blobFactory.MListMessages(listId);

            // store the data
            blobFactory.LInfo(listId).Set(info);
            bOwner.Set(ownerId);
            bFollowingAccounts.Set(followingAccounts);
            bFollowedAccounts.Set(new HashSet<Guid>());
            blobFactory.LFollowedAccountLockInit(listId);

            bMessages.Init();

            // add the lists to owned lists and check that the user exists. if he doesn't, delete the data stored
            if (!bOwned.AddWithRetry(listId))
            {
                bInfo.Delete();
                bOwner.Delete();
                bFollowingAccounts.Delete();
                bFollowedAccounts.Delete();
                blobFactory.LFollowedAccountLock(listId).Delete();
                bMessages.Delete();

                throw new AccountNotFound();
            }

            return listId;
        }

        // NYI
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Follow(Guid listId, Guid accountId)
        {
            using(blobFactory.LFollowedListsLock(accountId))
            {
                if (!blobFactory.LFollowingAccounts(listId).AddWithRetry(accountId))
                    throw new ListNotFound();

                blobFactory.LFollowedListsData(accountId).Add(listId);
            }
        }

        public void Unfollow(Guid listId, Guid accountId)
        {
            // check that the account is not the owner
            if (blobFactory.LOwner(listId).GetIfExists(new ListNotFound()) == accountId)
                throw new AccountIsOwner();

            using (blobFactory.LFollowedListsLock(accountId))
            {
                blobFactory.LFollowingAccounts(listId).RemoveWithRetry(accountId);
                blobFactory.LFollowedListsData(accountId).Remove(listId);
            }

        }

        public HashSet<Guid> GetAccounts(Guid listId)
        {
            return blobFactory.LFollowedAccountsData(listId).GetIfExists(new ListNotFound());
        }

        public HashSet<Guid> GetFollowingLists(Guid accountId)
        {
            return blobFactory.LFollowedByPublic(accountId).GetIfExists(new AccountNotFound());
        }

        public void Add(Guid listId, Guid accountId)
        {
            using (blobFactory.LFollowedAccountLock(listId))
            {
                if (!blobFactory.LFollowedByAll(accountId).AddWithRetry(listId))
                    throw new AccountNotFound();

                // check if the list is private or not
                // the account exists because it would need to take the mutex to be deleted
                if (!blobFactory.LInfo(listId).Get().IsPrivate)
                    blobFactory.LFollowedByPublic(accountId).AddWithRetry(listId);

                blobFactory.LFollowedAccountsData(listId).Add(accountId);
            }

        }

        public void Remove(Guid listId, Guid accountId)
        {
            // We don't check wether the list is private or not because it would be much more complicated and slower
            // it is much easier to remove the list form FollowingLists even if she doesn't belong to this set

            try
            {
                using (blobFactory.LFollowedAccountLock(listId))
                {
                    blobFactory.LFollowedByPublic(accountId).RemoveWithRetry(listId);
                    blobFactory.LFollowedByAll(accountId).RemoveWithRetry(listId);
                    blobFactory.LFollowedAccountsData(listId).Remove(accountId);
                }
            }
            catch (AccountNotFound) { }
        }

        public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
        {
            HashSet<Guid> lists = blobFactory.LOwnedListsPublic(accountId).GetIfExists(new AccountNotFound());

            if(withPrivate)
                lists = (HashSet<Guid>) lists.Concat(blobFactory.LOwnedListsPrivate(accountId).GetIfExists(new AccountNotFound()));

            return lists;
        }

        public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
        {
            HashSet<Guid> lists = blobFactory.LOwnedListsPublic(accountId).GetIfExists(new AccountNotFound());
            lists.UnionWith(blobFactory.LFollowedListsData(accountId).GetIfExists(new AccountNotFound()));

            if (withPrivate)
                lists = (HashSet<Guid>)lists.Concat(blobFactory.LOwnedListsPrivate(accountId).GetIfExists(new AccountNotFound()));

            return lists;
        }

        public HashSet<Guid> GetFollowingAccounts(Guid listId)
        {
            return blobFactory.LFollowingAccounts(listId).GetIfExists(new ListNotFound());
        }
    }
}
