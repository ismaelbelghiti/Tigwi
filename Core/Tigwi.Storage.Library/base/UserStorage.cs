using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Tigwi.Storage.Library;
using Tigwi.Storage.Library.Utilities;


namespace Tigwi.Storage.Library
{
    public class UserStorage : IUserStorage
    {
        BlobFactory blobFactory;

        // Constuctor
        public UserStorage(BlobFactory blobFactory)
        {
            this.blobFactory = blobFactory;
        }

        // Interface implementation
        public Guid GetId(string login)
        {
            Blob<Guid> blob = blobFactory.UIdByLogin(login);
            Guid id = blob.GetIfExists(new UserNotFound());
            if (id.Equals(Guid.Empty))
                throw new UserNotFound();
            return id;
        }

        public IUserInfo GetInfo(Guid userId)
        {
            return blobFactory.UInfo(userId).GetIfExists(new UserNotFound());
        }

        public void SetInfo(Guid userId, string email)
        {
            Blob<UserInfo> bInfo = blobFactory.UInfo(userId);
            UserInfo info = bInfo.GetIfExists(new UserNotFound());
            info.Email = email;
            if (!bInfo.SetIfExists(info))
                throw new UserNotFound();
        }

        public HashSet<Guid> GetAccounts(Guid userId)
        {
            return blobFactory.UAccountsData(userId).GetIfExists(new UserNotFound());
        }

        public Guid Create(string login, string email, Byte[] password)
        {
            // TODO unreserve the name if an error occure

            // reserve the name
            Blob<Guid> bLoginById = blobFactory.UIdByLogin(login);

            if (!bLoginById.SetIfNotExists(Guid.Empty))
                throw new UserAlreadyExists();

            // create the data
            Guid userId = Guid.NewGuid();
            // TODO : add the avatar
            UserInfo info = new UserInfo(login, "", email);
            HashSet<Guid> accounts = new HashSet<Guid>();

            // init blobs
            Blob<UserInfo> bInfo = blobFactory.UInfo(userId);
            Blob<HashSet<Guid>> bAccounts = blobFactory.UAccountsData(userId);
            Blob<ByteArray> bPassword = blobFactory.UPassword(userId);
            blobFactory.UAccountsLockInit(userId);
            blobFactory.UOpenIdsLockInit(userId);

            // store the data
            bInfo.Set(info);
            bAccounts.Set(accounts);
            bPassword.Set(new ByteArray(password));

            // we finish by unlocking the name
            bLoginById.Set(userId);

            return userId;
        }

        public void Delete(Guid userId)
        {
            // TODO : to be implemented
            throw new NotImplementedException();
        }

        public Guid GetIdByOpenIdUri(string openIdUri)
        {
            Blob<Guid> blob = blobFactory.UIdByOpenIdUri(openIdUri);
            Guid id = blob.GetIfExists(new UserNotFound());
            if (id.Equals(Guid.Empty))
                throw new UserNotFound();
            return id;
        }

        public void AssociateOpenIdUri(Guid userId, string openIdUri)
        {
            using (blobFactory.UOpenIdsLock(userId))
            {
                if (blobFactory.UIdByOpenIdUri(openIdUri).Exists)
                    throw new OpenIdUriDuplicated();

                if (blobFactory.UOpenIdsData(userId).Exists)
                    blobFactory.UOpenIdsData(userId).AddWithRetry(openIdUri);
                else
                {
                    HashSet<string> newset = new HashSet<string>();
                    newset.Add(openIdUri);
                    blobFactory.UOpenIdsData(userId).Set(newset);
                }

                blobFactory.UIdByOpenIdUri(openIdUri).Set(userId);
            }
        }

        public HashSet<string> ListOpenIdUris(Guid userId)
        {
            if (!blobFactory.UOpenIdsData(userId).Exists)
                throw new UserNotFound();

            return blobFactory.UOpenIdsData(userId).Get();
        }

        public void DeassociateOpenIdUri(Guid userId, string openIdUri)
        {
                using (blobFactory.UOpenIdsLock(userId))
                {
                    blobFactory.UOpenIdsData(userId).RemoveWithRetry(openIdUri);
                    blobFactory.UIdByOpenIdUri(openIdUri).Delete();
                }
        }

        public Byte[] GetPassword(Guid userId)
        {
            return blobFactory.UPassword(userId).GetIfExists(new UserNotFound()).Bytes;
        }

        public void SetPassword(Guid userId, Byte[] pass)
        {
            if (!blobFactory.UPassword(userId).SetIfExists(new ByteArray(pass)))
                throw new UserNotFound();
        }
    }
}
