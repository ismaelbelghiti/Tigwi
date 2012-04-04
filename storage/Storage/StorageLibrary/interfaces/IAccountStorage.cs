using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IAccountStorage
    {
        /// <summary>
        /// Return the id of an account given its name
        /// Can throw : AccountNotFound
        /// </summary>
        Guid GetId(string name);
        /// <summary>
        /// Return the infos related to the given account
        /// Can throw : AccountNotFound
        /// </summary>
        IAccountInfo GetInfo(Guid accountId);
        /// <summary>
        /// Update informations about the given account
        /// Can throw : AccountNotFound, AccountAlreadyExists
        /// </summary>
        void SetInfo(Guid accountId, string name, string description);

        /// <summary>
        /// Get the users who can post with the given account
        /// Can throw : AccountNotFound
        /// </summary>
        HashSet<Guid> GetUsers(Guid accountId);

        /// <summary>
        /// Get the admin of the given account
        /// Can throw : AccountNotFound
        /// </summary>
        Guid GetAdminId(Guid accountId);
        /// <summary>
        /// Set the admin of the given account
        /// Can throw : AccountNotFound, UserNotFound
        /// </summary>
        void SetAdminId(Guid accountId, Guid userId);
        
        /// <summary>
        /// Give the given user the right to post with the given account
        /// Can throw : AccountNotFound, UserNotFound
        /// </summary>
        void Add(Guid accountId, Guid userId);
        /// <summary>
        /// Remove the right to the given user to post with the given account
        /// Can throw : UserIsAdmin
        /// </summary>
        void Remove(Guid accountId, Guid userId);

        /// <summary>
        /// Create an account and return its ID
        /// Can throw : UserNotFound, AccountAlreadyExists
        /// </summary>
        Guid Create(Guid adminId, string name, string description);
        /// <summary>
        /// Delete the given account
        /// </summary>
        void Delete(Guid accountId);
    }
}
