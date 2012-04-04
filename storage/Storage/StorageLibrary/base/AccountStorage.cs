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
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.accountContainer, "idbyname/" + name);
            return blob.GetIfExists(new AccountNotFound());
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            StrgBlob<IAccountInfo> blob = new StrgBlob<IAccountInfo>(connexion.accountContainer, "info/" + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public void SetInfo(Guid accountId, string name, string description)
        {
            // To be moved to the worker

            // check name avaibility
            StrgBlob<Guid> nameIdBlob = new StrgBlob<Guid>(connexion.accountContainer, "idbyname/" + name);
            if (nameIdBlob.Exists && nameIdBlob.Get() != accountId)
                throw new AccountAlreadyExists();

            // store new info
            StrgBlob<IAccountInfo> bAccountInfo = new StrgBlob<IAccountInfo>(connexion.accountContainer, "info/" + accountId);
            if (!bAccountInfo.SetIfExsits(new AccountInfo(name, description)))
                throw new AccountNotFound();
        }

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, "users/" + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public Guid GetAdminId(Guid accountId)
        {
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.accountContainer, "adminid/" + accountId);
            return blob.GetIfExists(new AccountNotFound());
        }

        public void SetAdminId(Guid accountId, Guid userId)
        {
            // To be moved to the worker

            // check admin existence
            StrgBlob<IUserInfo> userInfo = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + userId);
            if (!userInfo.Exists)
                throw new UserNotFound();

            // store the new admin id
            StrgBlob<Guid> bAdminId = new StrgBlob<Guid>(connexion.accountContainer, "adminid/" + accountId);
            if (!bAdminId.SetIfExsits(userId))
                throw new AccountNotFound();
        }

        public void Add(Guid accountId, Guid userId)
        {
            // To be moved to the worker

            // check userId
            StrgBlob<IUserInfo> userInfo = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + userId);
            if (!userInfo.Exists)
                throw new UserNotFound();

            // update the data in account
            StrgBlob<HashSet<Guid>> bUsers = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, "users/" + accountId);
            HashSet<Guid> users = bUsers.GetIfExists(new AccountNotFound());
            users.Add(userId);
            bUsers.Set(users);

            // update the data in user - we have already check account existence while updating its value
            StrgBlob<HashSet<Guid>> bAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, "accounts/" + userId);
            HashSet<Guid> accounts = bAccounts.Get();
            accounts.Add(accountId);
            bUsers.Set(accounts);
        }

        public void Remove(Guid accountId, Guid userId)
        {
            // To be moved to the worker

            // TODO : implement
            throw new NotImplementedException();
        }

        public Guid Create(Guid adminId, string name, string description)
        {
            // To be transfered to the worker
            // TODO : add additional data that we need when we create lists and msgs

            // check name avaibility
            StrgBlob<Guid> nameIdBlob = new StrgBlob<Guid>(connexion.accountContainer, "idbyname/" + name);
            if (nameIdBlob.Exists)
                throw new AccountAlreadyExists();

            // check admin existence
            StrgBlob<IUserInfo> userInfo = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + adminId);
            if (!userInfo.Exists)
                throw new UserNotFound();

            // Create the data
            Guid id = new Guid();
            IAccountInfo info = new AccountInfo(name, description);
            HashSet<Guid> users = new HashSet<Guid>();

            // initialize blobs
            StrgBlob<Guid> bId = new StrgBlob<Guid>(connexion.accountContainer, "idbyname/" + name);
            StrgBlob<Guid> bAdminId = new StrgBlob<Guid>(connexion.accountContainer, "adminid/" + id);
            StrgBlob<IAccountInfo> bInfo = new StrgBlob<IAccountInfo>(connexion.accountContainer, "info/" + id);
            StrgBlob<HashSet<Guid>> bUsers = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, "users/" + id);

            // store the data
            bAdminId.Set(adminId);
            bUsers.Set(users);
            bInfo.Set(info);
            bId.Set(id);

            return id;
        }

        public void Delete(Guid accountId)
        {
            // TODO : implement
            throw new NotImplementedException();
        }
    }
}
