using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IUserStorage
    {
        int GetId(string login);
        IUserInfo GetInfo(int userId);
        void SetInfo(int userId, string login, string email);

        HashSet<int> GetAccounts(int userId);
        int Create(string login, string email);
        void Delete(int userId);
    }
}
