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
        /// </summary>
        /// <exception cref="UserNotFound"> if no user has this login</exception>
        Guid GetId(string login);

        /// <summary>
        /// get info related to the given user
        /// </summary>
        /// <exception cref="UserNotFound">if no user has this ID</exception>
        IUserInfo GetInfo(Guid userId);

        /// <summary>
        /// Set the infos related to the given user
        /// </summary>
        /// <exception cref="UserNotFound">if no user has this ID</exception>
        void SetInfo(Guid userId, string email);

        /// <summary>
        /// get the accounts where the user can post
        /// </summary>
        /// <exception cref="UserNotFound">if no user has this ID</exception>
        HashSet<Guid> GetAccounts(Guid userId);

        /// <summary>
        /// create a user
        /// </summary>
        /// <exception cref="UserAlreadyExists">if the login is already used</exception>
        Guid Create(string login, string email, string password);
       
        /// <summary>
        /// delete a user
        /// don't do anything if the user doesn't exists
        /// </summary>
        void Delete(Guid userId);

        // TODO : implement this
        /// <summary>
        /// To be used to check a user password
        /// </summary>
        /// <exception cref="UserNotFound"></exception>
        string GetPassword(Guid userID);

        /// <summary>
        /// Change a user password
        /// </summary>
        /// <exception cref="UserNotFound"></exception>
        void SetPassword(Guid userID, string password);
    }
}
