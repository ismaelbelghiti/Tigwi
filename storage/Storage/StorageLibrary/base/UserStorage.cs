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
        public int GetId(string login)
        {
            StrgBlob<int> blob = new StrgBlob<int>(connexion.userContainer, "idbylogin/" + login);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.UserNotFound));
        }

        public IUserInfo GetInfo(int userId)
        {
            StrgBlob<IUserInfo> blob = new StrgBlob<IUserInfo>(connexion.userContainer, "info/" + userId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.UserNotFound));
        }

        public void SetInfo(int userId, string login, string email)
        {
            // TODO : not thread safe and no error handling - for test purpose only
            CloudBlob blob = connexion.userContainer.GetBlobReference("info/" + userId);
            BlobStream stream = blob.OpenWrite();

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, new UserInfo(login, email));

            stream.Close();
        }

        public HashSet<int> GetAccounts(int userId)
        {
            StrgBlob<HashSet<int>> blob = new StrgBlob<HashSet<int>>(connexion.userContainer, "accounts/" + userId);
            return blob.GetIfExists(new StorageLibException(StrgLibErr.UserNotFound));
        }

        public int Create(string login, string email)
        {
            throw new NotImplementedException();
        }

        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

    }
}
