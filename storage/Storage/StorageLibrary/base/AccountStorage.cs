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
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.accountContainer, "idbylogin/" + name);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public IAccountInfo GetInfo(Guid accountId)
        {
            StrgBlob<IAccountInfo> blob = new StrgBlob<IAccountInfo>(connexion.accountContainer, "info/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public void SetInfo(Guid accountId, string name, string description)
        {
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
            throw new NotImplementedException();
        }

        public void Add(Guid accountId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid accountId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Guid Create(Guid adminId, string name, string description)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}
