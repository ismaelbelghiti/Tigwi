using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IAccountStorage
    {
        // TODO : specify the exceptions that each methodes can throw
        int GetId(string name);
        IAccountInfo GetInfo(int accountId);
        void SetInfo(int accountId, string name, string description);

        HashSet<int> GetUsers(int accountId);
        int GetAdminId(int accountId);
        void SetAdminId(int accountId);
        
        void Add(int accountId, int userId);
        void Remove(int accountId, int userId);

        int Create(int adminId, string name, string description);
        void Delete(int accountId);
    }
}
