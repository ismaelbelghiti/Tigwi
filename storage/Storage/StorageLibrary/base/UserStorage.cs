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
            Guid loginHash = Hasher.Hash(login);
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + loginHash);
            Guid id = blob.GetIfExists(new UserNotFound());
            if (id.Equals(Guid.Empty))
                throw new UserNotFound();
            return id;
        }

        public IUserInfo GetInfo(Guid userId)
        {
            StrgBlob<IUserInfo> blob = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + userId);
            return blob.GetIfExists(new UserNotFound());
        }

        public void SetInfo(Guid userId, string login, string email)
        {
            // Get login hash
            Guid LoginHash = Hasher.Hash(login);
            StrgBlob<Guid> bLoginById = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + LoginHash);

            using(new Mutex(connexion.userContainer, "locklogin/main"))
            {
                // WIP
            }

            throw new NotImplementedException();
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.userContainer, "accounts/" + userId);
            return blob.GetIfExists(new UserNotFound());
        }

        public Guid Create(string login, string email)
        {
            // TODO unreserve the name if an error occure
            // TODO scalable mutex for logins

            // reserve the name
            Guid loginHash = Hasher.Hash(login);
            StrgBlob<Guid> bLoginById = new StrgBlob<Guid>(connexion.userContainer, "idbylogin/" + loginHash);
            using (Mutex nameLock = new Mutex(connexion.userContainer, "locklogin/main"))
            {
                if (!bLoginById.SetIfNotExists(Guid.Empty))
                    throw new UserAlreadyExists();
            }

            // create the data
            Guid id = Guid.NewGuid();
            UserInfo info = new UserInfo(login, email);
            HashSet<Guid> accounts = new HashSet<Guid>();

            // init blobs
            StrgBlob<IUserInfo> bInfo = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + id);
            StrgBlob<HashSet<Guid>> bAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, "accounts/" + id + "/data");
            Mutex.InitMutex(connexion.userContainer, "accounts/" + id + "/lock");

            // store the data
            bInfo.Set(info);
            bAccounts.Set(accounts);

            // we finish by unlocking the name
            bLoginById.Set(id);

            return id;
        }

        public void Delete(Guid userId)
        {
            // TODO : to be implemented
            throw new NotImplementedException();
        }
    }
}
