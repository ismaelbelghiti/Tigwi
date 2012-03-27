using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;


namespace StorageLibrary
{
    public class UserStorage : IUserStorage
    {
        Storage storageAcces;

        // Constuctor
        public UserStorage(Storage storageAcces)
        {
            this.storageAcces = storageAcces;
        }

        // Interface implementation
        public int GetId(string login)
        {
            throw new NotImplementedException();
        }

        public IUserInfo GetInfo(int userId)
        {
            CloudBlob blob = storageAcces.userContainer.GetBlobReference("info/" + userId);
            BlobStream stream = blob.OpenRead();

            BinaryFormatter formatter = new BinaryFormatter();
            IUserInfo info = (UserInfo)formatter.Deserialize(stream);

            stream.Close();
            return info;
        }

        public void SetInfo(int userId, string login, string email)
        {
            CloudBlob blob = storageAcces.userContainer.GetBlobReference("info/" + userId);
            BlobStream stream = blob.OpenWrite();

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, new UserInfo(login, email));

            stream.Close();
        }

        public HashSet<int> GetAccounts(int userId)
        {
            throw new NotImplementedException();
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
