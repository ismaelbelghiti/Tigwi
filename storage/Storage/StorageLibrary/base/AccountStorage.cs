using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary;
using StorageLibrary.Utilities;

namespace StorageLibrary
{
    public class AccountStorage : IAccountStorage
    {
        StrgConnexion connexion;

        // Constuctor
        public AccountStorage(StrgConnexion connexion)
        {
            this.connexion = connexion;
        }

        // Interface implementation
        public Guid GetId(string name)
        {
            Blob<Guid> blob = new Blob<Guid>(connexion.accountContainer, Path.A_IDBYNAME + Hasher.Hash(name));
            return blob.GetIfExists(new AccountNotFound());
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            Blob<IAccountInfo> blob = new Blob<IAccountInfo>(connexion.accountContainer, Path.A_INFO + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public void SetInfo(Guid accountId, string description)
        {
            Blob<AccountInfo> bInfo = new Blob<AccountInfo>(connexion.accountContainer, Path.A_INFO + accountId);
            AccountInfo info = bInfo.GetIfExists(new AccountNotFound());
            info.Description = description;
            if (!bInfo.SetIfExists(info))
                throw new AccountNotFound();
        }

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            Blob<HashSet<Guid>> blob = new Blob<HashSet<Guid>>(connexion.accountContainer, Path.A_USERS + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public Guid GetAdminId(Guid accountId)
        {
            Blob<Guid> blob = new Blob<Guid>(connexion.accountContainer, Path.A_ADMINID + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public void SetAdminId(Guid accountId, Guid userId)
        {
            Blob<Guid> bAdmin = new Blob<Guid>(connexion.accountContainer, Path.A_ADMINID + accountId);
            Blob<HashSet<Guid>> bUserAccounts = new Blob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_DATA);

            using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_LOCK, new UserNotFound()))
            {
                HashSet<Guid> userAccounts = bUserAccounts.Get();
                // we check if the user is already on this account
                if (!userAccounts.Contains(accountId))
                {
                    HashSetBlob<Guid> accountUsers = new HashSetBlob<Guid>(connexion.accountContainer, Path.A_USERS + accountId);
                    if (!accountUsers.Add(userId))
                        throw new AccountNotFound();

                    userAccounts.Add(accountId);
                    bUserAccounts.Set(userAccounts);
                }

                if (!bAdmin.SetIfExists(userId))
                {
                    userAccounts.Remove(accountId);
                    bUserAccounts.Set(userAccounts);
                    throw new AccountNotFound();
                }
            }
        }

        public void Add(Guid accountId, Guid userId)
        {
            Blob<HashSet<Guid>> bUserAccounts = new Blob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_DATA);

            using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_LOCK, new UserNotFound()))
            {
                HashSetBlob<Guid> accountUsers = new HashSetBlob<Guid>(connexion.accountContainer, Path.A_USERS + accountId);
                if (!accountUsers.Add(userId))
                    throw new AccountNotFound();

                HashSet<Guid> userAccounts = bUserAccounts.Get();
                userAccounts.Add(accountId);
                bUserAccounts.Set(userAccounts);

                // not necessary ????
                //if (!accountUsers.Exists)
                //{
                //    userAccounts.Remove(accountId);
                //    bUserAccounts.Set(userAccounts);
                //    throw new AccountNotFound();
                //}
            }
        }

        public void Remove(Guid accountId, Guid userId)
        {
            Blob<HashSet<Guid>> bUserAccounts = new Blob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_DATA);
            Blob<Guid> bAdminId = new Blob<Guid>(connexion.accountContainer, Path.A_ADMINID + accountId);

            try
            {
                using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_LOCK, new UserNotFound()))
                {
                    if (bAdminId.GetIfExists(new AccountNotFound()).Equals(userId))
                        throw new UserIsAdmin();

                    HashSetBlob<Guid> accountUsers = new HashSetBlob<Guid>(connexion.accountContainer, Path.A_USERS + accountId);
                    accountUsers.Remove(userId);


                    HashSet<Guid> userAccounts = bUserAccounts.Get();
                    userAccounts.Remove(accountId);
                    bUserAccounts.Set(userAccounts);
                }

            }
            catch (UserNotFound) { }
            catch (AccountNotFound) { }
            
        }

        public Guid Create(Guid adminId, string name, string description)
        {
            // TODO : create the personnal list 

            Guid nameHash = Hasher.Hash(name);
            Blob<Guid> bNameById = new Blob<Guid>(connexion.accountContainer, Path.A_IDBYNAME + nameHash);

            if (!bNameById.SetIfNotExists(Guid.Empty))
                throw new AccountAlreadyExists();

            // create the data
            Guid id = Guid.NewGuid();
            AccountInfo info = new AccountInfo(name, description);
            HashSet<Guid> users = new HashSet<Guid>();
            users.Add(adminId);

            // init blobs
            Blob<IAccountInfo> bInfo = new Blob<IAccountInfo>(connexion.accountContainer, Path.A_INFO + id);
            Blob<HashSet<Guid>> bAccountUsers = new Blob<HashSet<Guid>>(connexion.accountContainer, Path.A_USERS + id);
            Blob<Guid> bAdminId = new Blob<Guid>(connexion.accountContainer, Path.A_ADMINID + id);

            Blob<HashSet<Guid>> bUserAccounts = new Blob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + adminId + Path.U_ACC_DATA);

            Blob<HashSet<Guid>> bOwnedListsPublic = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PUBLIC + id);
            Blob<HashSet<Guid>> bOwnedListsPrivate = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_OWNEDLISTS_PRIVATE + id);
            Blob<HashSet<Guid>> bFollowedLists = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDLISTS + id + Path.L_FOLLOWEDLISTS_DATA);
            Blob<HashSet<Guid>> bFollowedBy = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDBY + id);
            MsgSetBlobPack bTaggedMsg = new MsgSetBlobPack(connexion.msgContainer, Path.M_TAGGEDMESSAGES + id);

            // TODO : we could do it without a lock - or at least store the data before
            using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + adminId + Path.U_ACC_LOCK, new UserNotFound()))
            {
                // store the data
                bInfo.Set(info);
                bAccountUsers.Set(users);
                bAdminId.Set(adminId);
                bOwnedListsPublic.Set(new HashSet<Guid>());
                bOwnedListsPrivate.Set(new HashSet<Guid>());
                bFollowedLists.Set(new HashSet<Guid>());
                bFollowedBy.Set(new HashSet<Guid>());

                bTaggedMsg.Init();

                // Setup the personnal list
                Guid personnalListId = Guid.NewGuid();
                new Blob<Guid>(connexion.listContainer, Path.L_PERSO + id).Set(personnalListId);
                new Blob<ListInfo>(connexion.listContainer, Path.L_INFO + personnalListId).Set(new ListInfo("", "", true, true));
                new Blob<Guid>(connexion.listContainer, Path.L_OWNER + personnalListId).Set(id);
                Mutex.Init(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + personnalListId + Path.L_FOLLOWEDACC_DATA);
                Blob<HashSet<Guid>> bFollowedAccounts = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + personnalListId + Path.L_FOLLOWEDACC_DATA);
                HashSet<Guid> persoListFollowed = new HashSet<Guid>();
                persoListFollowed.Add(id);
                bFollowedAccounts.Set(persoListFollowed);
                new MsgSetBlobPack(connexion.msgContainer, Path.M_LISTMESSAGES + personnalListId).Init();

                Mutex.Init(connexion.listContainer, Path.L_FOLLOWEDLISTS + id + Path.L_FOLLOWEDLISTS_LOCK);

                // we finish by unlocking the name
                bNameById.Set(id);

                HashSet<Guid> userAccounts = bUserAccounts.Get();
                userAccounts.Add(id);
                bUserAccounts.Set(userAccounts);
            }

            return id;
        }

        public void Delete(Guid accountId)
        {
            // TODO : implement
            throw new NotImplementedException();
        }
    }
}
