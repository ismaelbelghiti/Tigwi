using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public class AccountStorage : IAccountStorage
    {
        Storage storageAcces;

        // Constuctor
        public AccountStorage(Storage storageAcces)
        {
            this.storageAcces = storageAcces;
        }

        // Interface implementation
        public int GetId(string name)
        {
            throw new NotImplementedException();
        }

        public IAccountInfo GetInfo(int accountId)
        {
            throw new NotImplementedException();
        }

        public void SetInfo(int accountId, string name, string description)
        {
            throw new NotImplementedException();
        }

        public HashSet<int> GetUsers(int accountId)
        {
            throw new NotImplementedException();
        }

        public int GetAdminId(int accountId)
        {
            throw new NotImplementedException();
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
