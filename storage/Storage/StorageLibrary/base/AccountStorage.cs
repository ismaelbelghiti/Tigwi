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
        public int GetId(string name)
        {
            StrgBlob<int> blob = new StrgBlob<int>(connexion.accountContainer, "idbylogin/" + name);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public IAccountInfo GetInfo(int accountId)
        {
            StrgBlob<IAccountInfo> blob = new StrgBlob<IAccountInfo>(connexion.accountContainer, "info/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public void SetInfo(int accountId, string name, string description)
        {
            throw new NotImplementedException();
        }

        public HashSet<int> GetUsers(int accountId)
        {
            StrgBlob<HashSet<int>> blob = new StrgBlob<HashSet<int>>(connexion.accountContainer, "users/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public int GetAdminId(int accountId)
        {
            StrgBlob<int> blob = new StrgBlob<int>(connexion.accountContainer, "adminid/" + accountId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.AccountNotFound));
        }

        public void SetAdminId(int accountId)
        {
            throw new NotImplementedException();
        }

        public void Add(int accountId, int userId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int accountId, int userId)
        {
            throw new NotImplementedException();
        }

        public int Create(int adminId, string name, string description)
        {
            throw new NotImplementedException();
        }

        public void Delete(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
