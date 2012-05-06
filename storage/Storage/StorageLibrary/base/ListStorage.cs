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
        BlobFactory blobFactory;

        // Constuctor
        public ListStorage(BlobFactory blobFactory)
        {
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
            Blob<ListInfo> bInfo = blobFactory.LInfo(listId);
            Blob<Guid> bOwner = blobFactory.LOwner(listId);
            HashSetBlob<Guid> bOwned = isPrivate ? blobFactory.LOwnedListsPrivate(ownerId) : blobFactory.LOwnedListsPublic(ownerId);
            Blob<HashSet<Guid>> bFollowingAccounts = blobFactory.LFollowingAccounts(listId);
            Blob<HashSet<Guid>> bFollowedAccounts = blobFactory.LFollowedAccounts(listId);
            MsgSetBlobPack bMessages = blobFactory.MListMessages(listId);

            // store the data
            blobFactory.LInfo(listId).Set(info);
            bOwner.Set(ownerId);
            bFollowingAccounts.Set(followingAccounts);
            bFollowedAccounts.Set(new HashSet<Guid>());

            bMessages.Init();

            // add the lists to owned lists and check that the user exists. if he doesn't, delete the data stored
            if (!bOwned.AddWithRetry(listId))
            {
                bInfo.Delete();
                bOwner.Delete();
                bFollowingAccounts.Delete();
                bFollowedAccounts.Delete();
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
            return blobFactory.LFollowedAccounts(listId).GetIfExists(new ListNotFound());
        }

        public HashSet<Guid> GetFollowingLists(Guid accountId)
        {
            return blobFactory.LFollowedByPublic(accountId).GetIfExists(new AccountNotFound());
        }

        public void Add(Guid listId, Guid accountId)
        {
            HashSetBlob<Guid> bLFollowedByAll = blobFactory.LFollowedByAll(accountId);

            // we do this even if we're not sure the list exists
            // if it doesn't, we will remove it by the end of this function
            HashSet<Guid> set;
            do
            {
                set = bLFollowedByAll.GetIfExists(new AccountNotFound());
                if (set.Contains(listId))
                    return;
                set.Add(listId);
            } while (!bLFollowedByAll.TrySet(set));

            // The account now can't be removed until the request is completed

            Guid PersonnalListId = blobFactory.LPersonnalList(accountId).GetIfExists(new AccountNotFound());
            if (!blobFactory.MListMessages(listId).UnionWith(blobFactory.MListMessages(PersonnalListId))
                || !blobFactory.LFollowedAccounts(listId).AddWithRetry(accountId))
            {
                bLFollowedByAll.RemoveWithRetry(listId);
                throw new ListNotFound();
            }

            // we don't need to check the account or list existence since the request needs to be ended bedore
            if (!blobFactory.LInfo(listId).Get().IsPrivate)
                blobFactory.LFollowedByPublic(accountId).AddWithRetry(listId);
        }

        // TODO : remove messages
        public void Remove(Guid listId, Guid accountId)
        {
            try
            {
                // Do the job if the list is public
                if (!blobFactory.LInfo(listId).GetIfExists(new ListNotFound()).IsPrivate)
                {
                    HashSet<Guid> LFollowedByPublic;
                    Blob<HashSet<Guid>> bLFollowedByPublic = blobFactory.LFollowedByPublic(accountId);
                    do
                    {
                        LFollowedByPublic = bLFollowedByPublic.GetIfExists(new AccountNotFound());

                        // check that we are not already working on this list
                        if (!LFollowedByPublic.Contains(listId))
                            return;

                        LFollowedByPublic.Remove(listId);
                    } while (!bLFollowedByPublic.TrySet(LFollowedByPublic));
                }

                // Remove from following accounts
                HashSet<Guid> LFollowedAccounts;
                Blob<HashSet<Guid>> bLFollowedAccounts = blobFactory.LFollowedAccounts(listId);
                do
                {
                    LFollowedAccounts = bLFollowedAccounts.GetIfExists(new ListNotFound());

                    // check that we are not already working on this list
                    if (!LFollowedAccounts.Contains(accountId))
                        return;

                    LFollowedAccounts.Remove(accountId);
                } while (!bLFollowedAccounts.TrySet(LFollowedAccounts));


                // TODO :
                // - remove from msgs
                // PROBLEM : we migh add msgs to the list while we try to remove them
                // SOLUTIONS : 
                // - hide them and remove them only after
                // - it would allow an add to cancel a remove
                // - problem : when do we remove them ?
                //      

                blobFactory.LFollowedByAll(accountId).RemoveWithRetry(listId);
            }
            catch { }
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
