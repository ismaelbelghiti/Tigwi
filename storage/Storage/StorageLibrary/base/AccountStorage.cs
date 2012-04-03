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
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            StrgBlob<IAccountInfo> blob = new StrgBlob<IAccountInfo>(connexion.accountContainer, "info/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public void SetInfo(Guid accountId, string name, string description)
        {
            // TODO : implement
            throw new NotImplementedException();
        }

        public HashSet<Guid> GetUsers(Guid accountId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.accountContainer, "users/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public Guid GetAdminId(Guid accountId)
        {
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.accountContainer, "adminid/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public void SetAdminId(Guid accountId, Guid userId)
        {
            // TODO : implement
            throw new NotImplementedException();
        }

        public void Add(Guid accountId, Guid userId)
        {
            // TODO : implement
            throw new NotImplementedException();
        }

        public void Remove(Guid accountId, Guid userId)
        {
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
                throw new StorageLibException(StrgLibErr.AccountAlreadyExist);

            // check admin existence
            StrgBlob<IUserInfo> userInfo = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + adminId);
            if (!userInfo.Exists)
                throw new StorageLibException(StrgLibErr.AccountNotFound);

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
