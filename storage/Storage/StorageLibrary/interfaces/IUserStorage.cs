using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IUserStorage
    {
        /// <summary>
        /// Get a user id given its login
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        Guid GetId(string login);

        /// <summary>
        /// get info related to the given user
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        IUserInfo GetInfo(Guid userId);

        /// <summary>
        /// Set the infos related to the given user
        /// Can raise : 
        /// UserNotFound
        /// UserAlreadyExists
        /// </summary>
        void SetInfo(Guid userId, string login, string email);

        /// <summary>
        /// get the accounts where the user can post
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        HashSet<Guid> GetAccounts(Guid userId);

        /// <summary>
        /// create a user
        /// Can raise : 
        /// UserAlreadyExists
        /// </summary>
        Guid Create(string login, string email);
       
        /// <summary>
        /// delete a user
        /// </summary>
        void Delete(Guid userId);
    }
}
