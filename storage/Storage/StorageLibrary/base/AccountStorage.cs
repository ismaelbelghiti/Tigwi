using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageCommon;

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
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.accountContainer, Path.A_IDBYNAME + Hasher.Hash(name));
            return blob.GetIfExists(new AccountNotFound());
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            StrgBlob<IAccountInfo> blob = new StrgBlob<IAccountInfo>(connexion.accountContainer, Path.A_INFO + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public void SetInfo(Guid accountId, string description)
        {
            StrgBlob<AccountInfo> bInfo = new StrgBlob<AccountInfo>(connexion.accountContainer, Path.A_INFO + accountId);
            AccountInfo info = bInfo.GetIfExists(new AccountNotFound());
            info.Description = description;
            if (!bInfo.SetIfExists(info))
                throw new AccountNotFound();
        }

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, Path.A_USERS + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public Guid GetAdminId(Guid accountId)
        {
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.accountContainer, Path.A_ADMINID + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public void SetAdminId(Guid accountId, Guid userId)
        {
            StrgBlob<Guid> bAdmin = new StrgBlob<Guid>(connexion.accountContainer, Path.A_ADMINID + accountId);
            StrgBlob<HashSet<Guid>> bUserAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_DATA);
            StrgBlob<HashSet<Guid>> bAccountUsers = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, Path.A_USERS + accountId);

            using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_LOCK, new UserNotFound()))
            {
                HashSet<Guid> userAccounts = bUserAccounts.Get();
                // we check if the user is already on this account
                if (userAccounts.Contains(accountId))
                {
                    // TODO : waiting for etag protection
                    HashSet<Guid> accountUsers = bAccountUsers.GetIfExists(new UserNotFound());
                    accountUsers.Add(userId);
                    bAccountUsers.Set(accountUsers);

                    userAccounts.Add(accountId);
                    bUserAccounts.Set(userAccounts);
                }

                // TODO : ajouter le mec comme user de l`account
                if(!bAdmin.SetIfExists(userId))
                    throw new AccountNotFound();
            }
        }

        public void Add(Guid accountId, Guid userId)
        {
            // waiting for mutex protection
            //using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_LOCK))
            {
                StrgBlob<HashSet<Guid>> bUsers = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, Path.A_USERS + accountId);
                StrgBlob<HashSet<Guid>> bAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId);

                HashSet<Guid> users = bUsers.GetIfExists(new AccountNotFound());
                HashSet<Guid> accounts = bAccounts.GetIfExists(new UserNotFound());

                users.Add(userId);
                accounts.Add(accountId);

                //if user or account destroyed?
                bUsers.Set(users);
                bAccounts.Set(accounts);
                 
            }
        }

        public void Remove(Guid accountId, Guid userId)
        {
            // To be moved to the worker

            // TODO : implement
            throw new NotImplementedException();
        }

        public Guid Create(Guid adminId, string name, string description)
        {
            Guid nameHash = Hasher.Hash(name);
            StrgBlob<Guid> bNameById = new StrgBlob<Guid>(connexion.accountContainer, Path.A_IDBYNAME + nameHash);

            if (!bNameById.SetIfNotExists(Guid.Empty))
                throw new AccountAlreadyExists();

            // create the data
            Guid id = Guid.NewGuid();
            AccountInfo info = new AccountInfo(name, description);
            HashSet<Guid> users = new HashSet<Guid>();

            // init blobs
            StrgBlob<IAccountInfo> bInfo = new StrgBlob<IAccountInfo>(connexion.accountContainer, Path.A_INFO + id);
            StrgBlob<HashSet<Guid>> bUsers = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, Path.A_USERS + id);
            StrgBlob<Guid> bAdminId = new StrgBlob<Guid>(connexion.accountContainer, Path.A_ADMINID + id);

            // store the data
            bInfo.Set(info);
            bUsers.Set(users);
            bAdminId.Set(adminId);

            // we finish by unlocking the name
            bNameById.Set(id);

            return id;
        }

        public void Delete(Guid accountId)
        {
            // TODO : implement
            throw new NotImplementedException();
        }
    }
}
