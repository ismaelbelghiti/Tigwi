using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IAccountStorage
    {
        // TODO : specify the exceptions that each methode can throw
        Guid GetId(string name);
        IAccountInfo GetInfo(Guid accountId);
        void SetInfo(Guid accountId, string name, string description);

        HashSet<Guid> GetUsers(Guid accountId);
        Guid GetAdminId(Guid accountId);
        void SetAdminId(Guid accountId);
        
        void Add(Guid accountId, Guid userId);
        void Remove(Guid accountId, Guid userId);

        Guid Create(Guid adminId, string name, string description);
        void Delete(Guid accountId);
    }
}
