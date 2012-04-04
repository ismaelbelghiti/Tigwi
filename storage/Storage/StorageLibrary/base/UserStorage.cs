using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using StorageCommon;

namespace StorageLibrary
{
    public class UserStorage : IUserStorage
    {
        StrgConnexion connexion;

        // Constuctor
        public UserStorage(StrgConnexion connexion)
        {
            this.connexion = connexion;
        }

        // Interface implementation
        public Guid GetId(string login)
        {
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + login);
            return blob.GetIfExists(new UserNotFound());
        }

        public IUserInfo GetInfo(Guid userId)
        {
            StrgBlob<IUserInfo> blob = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + userId);
            return blob.GetIfExists(new UserNotFound());
        }

        public void SetInfo(Guid userId, string login, string email)
        {
            // To be transfered to the worker

            // Check login avaibility
            StrgBlob<Guid> loginIdBlob = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + login);
            if (loginIdBlob.Exists && loginIdBlob.Get() != userId)
                throw new UserAlreadyExists();

            // store the data and check existence
            UserInfo info = new UserInfo(login, email);
            StrgBlob<UserInfo> blob = new StrgBlob<UserInfo>(connexion.userContainer, "info/" + userId);
            if (!blob.SetIfExsits(info))
                throw new UserNotFound();
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.userContainer, "accounts/" + userId);
            return blob.GetIfExists(new UserNotFound());
        }

        public Guid Create(string login, string email)
        {
            // To be transfered to the worker
            // TODO : complete the object created while we implement accounts, lists and messages

            // Check login avaibility
            StrgBlob<Guid> loginIdBlob = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + login);
            if (loginIdBlob.Exists)
                throw new UserAlreadyExists();

            // Create the user
            // initialize data
            Guid id = new Guid();
            IUserInfo info = new UserInfo(login, email);
            HashSet<Guid> accounts = new HashSet<Guid>();

            // initialize blobs
            StrgBlob<IUserInfo> bInfo = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + id);
            StrgBlob<Guid> bID = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + login);
            StrgBlob<HashSet<Guid>> bAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, "accounts/" + id);

            // Store the data in the right order for the user not to be accessible until the end
            bInfo.Set(info);
            bAccounts.Set(accounts);
            bID.Set(id);

            // return the id
            return id;
        }

        public void Delete(Guid userId)
        {
            // TODO : to be implemented
            throw new NotImplementedException();
        }
    }
}
