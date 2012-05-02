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
        Guid Create(string login, string email, Byte[] password);
       
        /// <summary>
        /// delete a user
        /// doesn't do anything if the user doesn't exists
        /// </summary>
        /// <exception cref="UserIsAdmin">To avoid deleting an account</exception>
        void Delete(Guid userId);

        /// <summary>
        /// Get a user id given an openid uri
        /// </summary>
        /// <exception cref="UserNotFound"> if no user has this openid uri</exception>
        Guid GetIdByOpenIdUri(string openIdUri);

        /// <summary>
        /// Associate an user to an openid uri
        /// </summary>
        /// <exception cref="UserNotFound"> if no user has this id</exception>
        /// <exception cref="OpenIdUriDuplicated"> if an user already has this openid uri</exception>
        Guid AssociateOpenIdUri(Guid userId, string openIdUri);

        /// <summary>
        /// List the openid uri associated to an user
        /// </summary>
        /// <exception cref="UserNotFound"> if not user has this id</exception>
        HashSet<string> ListOpenIdUris(Guid userId);

        /// <summary>
        /// Deassociate an openid uri from an user
        /// </summary>
        /// <exception cref="UserNotFound"> if no user has this id</exception>
        /// <exception cref="OpenIdUriNotAssociated"> if the given uri is not associated to the given user</exception>
        void DeassociateOpenIdUri(Guid userId, string openIdUri);

        // TODO : implement this
        /// <summary>
        /// To be used to check a user password
        /// </summary>
        /// <exception cref="UserNotFound"></exception>
        Byte[] GetPassword(Guid userId);

        /// <summary>
        /// Change a user password
        /// </summary>
        /// <exception cref="UserNotFound"></exception>
        void SetPassword(Guid userId, Byte[] password);
    }
}
