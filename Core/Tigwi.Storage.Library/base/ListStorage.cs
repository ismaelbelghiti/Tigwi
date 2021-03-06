﻿#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tigwi.Storage.Library;
using Tigwi.Storage.Library.Utilities;

namespace Tigwi.Storage.Library
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
        
        public void SetInfo(Guid listId, string name, string description, bool isPrivate)
        {
            // autorisation passage publique/privé
            // si passage publique privé : pseudo-suppression

            //ListInfo infos = blobFactory.LInfo(listId).GetIfExists(new ListNotFound());
            //infos.Description = description;
            //infos.Name = name;
            //if (infos.IsPersonnal)
            //    throw new IsPersonnalList();
            //blobFactory.LInfo(listId).Set(infos);

            Blob<ListInfo> bInfo = blobFactory.LInfo(listId); 
            ListInfo info = bInfo.GetIfExists(new ListNotFound());
            if (info.IsPersonnal)
                throw new IsPersonnalList();
            info.Description = description;
            info.Name = name;
            info.IsPrivate = isPrivate;
            if (!bInfo.SetIfExists(info))
                throw new ListNotFound();
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
            DictionaryBlob<Guid> bFollowedAccounts = blobFactory.LFollowedAccounts(listId);
            HashSetBlob<Guid> bAddRmvMsgs = blobFactory.LAddRmvMsgs(listId);
            MsgSetBlobPack bMessages = blobFactory.MListMessages(listId);

            // store the data
            blobFactory.LInfo(listId).Set(info);
            bOwner.Set(ownerId);
            bFollowingAccounts.Set(followingAccounts);
            bFollowedAccounts.Set(new Dictionary<Guid,bool>());
            bAddRmvMsgs.Set(new HashSet<Guid>());

            bMessages.Init();

            // add the lists to owned lists and check that the user exists. if he doesn't, delete the data stored
            if (!bOwned.AddWithRetry(listId))
            {
                bInfo.Delete();
                bOwner.Delete();
                bFollowingAccounts.Delete();
                bFollowedAccounts.Delete();
                bMessages.Delete();
                bAddRmvMsgs.Delete();

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
            return new HashSet<Guid>(blobFactory.LFollowedAccounts(listId).GetIfExists(new ListNotFound()).Keys);
        }

        public HashSet<Guid> GetMainAccounts(Guid listId)
        {
            Dictionary<Guid, bool> dict = blobFactory.LFollowedAccounts(listId).GetIfExists(new ListNotFound());
            return new HashSet<Guid>(dict.Keys.Where(account => dict[account]));
        }

        public void SetMain(Guid listId, Guid accountId, bool isMain)
        {
            if (!blobFactory.LFollowedAccounts(listId).SetBool(accountId, isMain))
                throw new ListNotFound();
        }

        public HashSet<Guid> GetFollowingLists(Guid accountId)
        {
            return blobFactory.LFollowedByPublic(accountId).GetIfExists(new AccountNotFound());
        }

        public void Add(Guid listId, Guid accountId)
        {
            // Set WIP to true
            HashSetBlob<Guid> bAddRmvMsgs = blobFactory.LAddRmvMsgs(listId);
            if (!bAddRmvMsgs.AddIfNotInWithRetry(accountId, new ListNotFound()))
                return;

            // Add list and accounts to the sets
            if (!blobFactory.LFollowedByAll(accountId).AddWithRetry(listId))
            {
                bAddRmvMsgs.RemoveWithRetry(accountId);
                throw new AccountNotFound();
            }

            blobFactory.LFollowedAccounts(listId).AddWithRetry(accountId);

            if (!blobFactory.LInfo(listId).Get().IsPrivate)
                blobFactory.LFollowedByPublic(accountId).AddWithRetry(listId);

            // add msgs
            Guid PersonnalListId = blobFactory.LPersonnalList(accountId).Get();
            blobFactory.MListMessages(listId).UnionWith(blobFactory.MListMessages(PersonnalListId));

            // set WIP to false
            bAddRmvMsgs.RemoveWithRetry(accountId);
        }

        public void Remove(Guid listId, Guid accountId)
        {
            try
            {
                HashSetBlob<Guid> bAddRmvMsgs = blobFactory.LAddRmvMsgs(listId);
                if(!bAddRmvMsgs.AddIfNotInWithRetry(accountId, new ListNotFound()))
                    return;

                if (!blobFactory.LFollowedByPublic(accountId).RemoveWithRetry(listId))
                {
                    bAddRmvMsgs.RemoveWithRetry(accountId);
                    throw new AccountNotFound();
                }

                blobFactory.LFollowedByAll(accountId).RemoveWithRetry(listId);
                blobFactory.LFollowedAccounts(listId).RemoveWithRetry(accountId);

                blobFactory.MListMessages(listId).ExceptWith(blobFactory.MListMessages(blobFactory.LPersonnalList(accountId).Get()));

                bAddRmvMsgs.RemoveWithRetry(accountId);
            }
            catch { }
        }

        public HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)
        {
            HashSet<Guid> lists = blobFactory.LOwnedListsPublic(accountId).GetIfExists(new AccountNotFound());

            if(withPrivate)
                lists.UnionWith(blobFactory.LOwnedListsPrivate(accountId).GetIfExists(new AccountNotFound()));

            return lists;
        }

        public HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)
        {
            HashSet<Guid> lists = blobFactory.LOwnedListsPublic(accountId).GetIfExists(new AccountNotFound());
            lists.UnionWith(blobFactory.LFollowedListsData(accountId).GetIfExists(new AccountNotFound()));

            if (withPrivate)
                lists.UnionWith(blobFactory.LOwnedListsPrivate(accountId).GetIfExists(new AccountNotFound()));

            return lists;
        }

        public HashSet<Guid> GetFollowingAccounts(Guid listId)
        {
            return blobFactory.LFollowingAccounts(listId).GetIfExists(new ListNotFound());
        }
    }
}
