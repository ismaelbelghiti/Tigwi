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
            return blob.GetIfExists(new StorageLibException(StrgLibErr.UserNotFound));
        }

        public IUserInfo GetInfo(Guid userId)
        {
            StrgBlob<IUserInfo> blob = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + userId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.UserNotFound));
        }

        public void SetInfo(Guid userId, string login, string email)
        {
            // To be transfered to the worker

            // Check login avaibility
            StrgBlob<Guid> loginIdBlob = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + login);
            if (loginIdBlob.Exists && loginIdBlob.Get() != userId)
                throw new StorageLibException(StrgLibErr.UserAlreadyExists);

            // store the data and check existence
            UserInfo info = new UserInfo(login, email);
            StrgBlob<UserInfo> blob = new StrgBlob<UserInfo>(connexion.userContainer, "info/" + userId);
            if (!blob.SetIfExsits(info))
                throw new StorageLibException(StrgLibErr.UserNotFound);
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.userContainer, "accounts/" + userId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.UserNotFound));
        }

        public Guid Create(string login, string email)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid userId)
        {
            // TODO : to be implemented
            throw new NotImplementedException();
        }
    }
}
