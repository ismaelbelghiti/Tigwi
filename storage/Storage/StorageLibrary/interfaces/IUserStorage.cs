using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IUserStorage
    {
        /// <summary>
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        int GetId(string login);

        /// <summary>
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        IUserInfo GetInfo(int userId);

        /// <summary>
        /// Can raise : 
        /// UserNotFound
        /// UserAlreadyExists
        /// </summary>
        void SetInfo(int userId, string login, string email);

        /// <summary>
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        HashSet<int> GetAccounts(int userId);

        /// <summary>
        /// Can raise : 
        /// UserAlreadyExists
        /// </summary>
        int Create(string login, string email);
       
        /// <summary>
        /// Dont do anything if the user is already deleted
        /// The user might not be deleted immediately
        /// </summary>
        void Delete(int userId);
    }
}
