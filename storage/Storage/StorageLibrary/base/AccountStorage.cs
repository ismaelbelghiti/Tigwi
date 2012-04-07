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

            StrgBlob<IUserInfo> bUser = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + userId);
            if(!bUser.Exists)
                throw new UserNotFound();

            if(!bAdmin.SetIfExists(userId))
                throw new AccountNotFound();
        }

        public void Add(Guid accountId, Guid userId)
        {
            throw new NotImplementedException();
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
