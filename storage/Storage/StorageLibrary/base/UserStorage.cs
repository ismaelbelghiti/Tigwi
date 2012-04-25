using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using StorageLibrary;
using StorageLibrary.Utilities;


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
            StrgBlob<Guid> blob = new StrgBlob<Guid>(connexion.userContainer, Path.U_IDBYLOGIN + loginHash);
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

        public void SetInfo(Guid userId, string email)
        {
            StrgBlob<UserInfo> bInfo = new StrgBlob<UserInfo>(connexion.userContainer, Path.U_INFO + userId);
            UserInfo info = bInfo.GetIfExists(new UserNotFound());
            info.Email = email;
            if (!bInfo.SetIfExists(info))
                throw new UserNotFound();
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            StrgBlob<HashSet<Guid>> blob = new StrgBlob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + userId + Path.U_ACC_DATA);
            return blob.GetIfExists(new UserNotFound());
        }

        public Guid Create(string login, string email, string password)
        {
            // TODO unreserve the name if an error occure

            // reserve the name
            Guid loginHash = Hasher.Hash(login);
            StrgBlob<Guid> bLoginById = new StrgBlob<Guid>(connexion.userContainer, Path.U_IDBYLOGIN + loginHash);

            if (!bLoginById.SetIfNotExists(Guid.Empty))
                throw new UserAlreadyExists();

            // create the data
            Guid id = Guid.NewGuid();
            // TODO : add the avatar
            UserInfo info = new UserInfo(login, "", email);
            HashSet<Guid> accounts = new HashSet<Guid>();

            // init blobs
            StrgBlob<IUserInfo> bInfo = new StrgBlob<IUserInfo>(connexion.userContainer, Path.U_INFO + id);
            StrgBlob<HashSet<Guid>> bAccounts = new StrgBlob<HashSet<Guid>>(connexion.userContainer, Path.U_ACCOUNTS + id + Path.U_ACC_DATA);
            StrgBlob<string> bPassword = new StrgBlob<string>(connexion.userContainer, Path.U_PASSWORD + id);
            Mutex.Init(connexion.userContainer, Path.U_ACCOUNTS + id + Path.U_ACC_LOCK);

            // store the data
            bInfo.Set(info);
            bAccounts.Set(accounts);
            bPassword.Set(password);

            // we finish by unlocking the name
            bLoginById.Set(id);

            return id;
        }

        public void Delete(Guid userId)
        {
            // TODO : to be implemented
            throw new NotImplementedException();
        }

        public string GetPassword(Guid userId)
        {
            StrgBlob<string> bPass = new StrgBlob<string>(connexion.userContainer, Path.U_PASSWORD + userId);
            return bPass.GetIfExists(new UserNotFound());
        }

        public void SetPassword(Guid userId, string pass)
        {
            StrgBlob<string> bPass = new StrgBlob<string>(connexion.userContainer, Path.U_PASSWORD + userId);
            if (!bPass.SetIfExists(pass))
                throw new UserNotFound();
        }
    }
}
