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
            StrgBlob<HashSet<Guid>> bUserAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_DATA);

            using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_LOCK, new UserNotFound()))
            {
                HashSetBlob<Guid> accountUsers = new HashSetBlob<Guid>(connexion.accountContainer, Path.A_USERS + accountId);
                if (!accountUsers.Add(userId))
                    throw new AccountNotFound();

                HashSet<Guid> userAccounts = bUserAccounts.Get();
                userAccounts.Add(accountId);
                bUserAccounts.Set(userAccounts);

                if (!accountUsers.Exists)
                {
                    userAccounts.Remove(accountId);
                    bUserAccounts.Set(userAccounts);
                    throw new AccountNotFound();
                }
            }
        }

        public void Remove(Guid accountId, Guid userId)
        {
            StrgBlob<HashSet<Guid>> bUserAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_DATA);
            StrgBlob<Guid> bAdminId = new StrgBlob<Guid>(connexion.accountContainer, Path.A_ADMINID + accountId);

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
            Guid nameHash = Hasher.Hash(name);
            StrgBlob<Guid> bNameById = new StrgBlob<Guid>(connexion.accountContainer, Path.A_IDBYNAME + nameHash);

            if (!bNameById.SetIfNotExists(Guid.Empty))
                throw new AccountAlreadyExists();

            // create the data
            Guid id = Guid.NewGuid();
            AccountInfo info = new AccountInfo(name, description);
            HashSet<Guid> users = new HashSet<Guid>();
            users.Add(adminId);

            // init blobs
            StrgBlob<IAccountInfo> bInfo = new StrgBlob<IAccountInfo>(connexion.accountContainer, Path.A_INFO + id);
            StrgBlob<HashSet<Guid>> bAccountUsers = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, Path.A_USERS + id);
            StrgBlob<Guid> bAdminId = new StrgBlob<Guid>(connexion.accountContainer, Path.A_ADMINID + id);
            StrgBlob<HashSet<Guid>> bUserAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + adminId + Path.U_ACC_DATA);

            using (new Mutex(connexion.userContainer, Path.U_ACCOUNTS + adminId + Path.U_ACC_LOCK, new UserNotFound()))
            {
                // store the data
                bInfo.Set(info);
                bAccountUsers.Set(users);
                bAdminId.Set(adminId);

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
