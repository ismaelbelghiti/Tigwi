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
        Guid GetId(string login);

        /// <summary>
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        IUserInfo GetInfo(Guid userId);

        /// <summary>
        /// Can raise : 
        /// UserNotFound
        /// UserAlreadyExists
        /// </summary>
        void SetInfo(Guid userId, string login, string email);

        /// <summary>
        /// Can raise : 
        /// UserNotFound
        /// </summary>
        HashSet<Guid> GetAccounts(Guid userId);

        /// <summary>
        /// Can raise : 
        /// UserAlreadyExists
        /// </summary>
        Guid Create(string login, string email);
       
        /// <summary>
        /// Dont do anything if the user is already deleted
        /// The user might not be deleted immediately
        /// </summary>
        void Delete(Guid userId);
    }
}
