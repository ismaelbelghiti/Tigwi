using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using StorageLibrary.Utilities;

namespace StorageLibrary
{
    // TODO : change this class to private
	public class BlobFactory
	{
        // Cloud Connection
        private CloudBlobContainer userContainer;
        private CloudBlobContainer accountContainer;
        private CloudBlobContainer listContainer;
        private CloudBlobContainer msgContainer;

        public BlobFactory(string azureAccountName, string azureKey)
        {
            // initialize Azure Account
            StorageCredentialsAccountAndKey storageKey = new StorageCredentialsAccountAndKey(azureAccountName, azureKey);
            CloudStorageAccount azureAccount = new CloudStorageAccount(storageKey, true);

            // Create blob containers
            CloudBlobClient blobClient = azureAccount.CreateCloudBlobClient();

            userContainer = blobClient.GetContainerReference("user");
            accountContainer = blobClient.GetContainerReference("account");
            listContainer = blobClient.GetContainerReference("list");
            msgContainer = blobClient.GetContainerReference("msg");
        }

        public void InitStorage()
        {
            userContainer.CreateIfNotExist();
            ClearContainer(userContainer);

            accountContainer.CreateIfNotExist();
            ClearContainer(accountContainer);

            listContainer.CreateIfNotExist();
            ClearContainer(listContainer);

            msgContainer.CreateIfNotExist();
            ClearContainer(msgContainer);
        }

        void ClearContainer(CloudBlobContainer c)
        {
            BlobRequestOptions opt = new BlobRequestOptions();
            opt.UseFlatBlobListing = true;
            foreach (CloudBlob blob in c.ListBlobs(opt))
                blob.Delete();
        }

        // User Blobs
        public Blob<Guid> UIdByLogin(string login)
        {
            Guid loginHash = Hasher.Hash(login);
            return new Blob<Guid>(userContainer, U_IDBYLOGIN + loginHash);
        }

        public Blob<IUserInfo> UInfo(Guid userId)
        {
            return new Blob<IUserInfo>(userContainer, U_INFO + userId);
        }

        public Blob<string> UPassword(Guid userId)
        {
            return new Blob<string>(userContainer, U_PASSWORD + userId);
        }

        public Mutex UAccountsLock(Guid userId)
        {
            return new Mutex(userContainer, U_ACCOUNTS + userId + LOCK, new UserNotFound());
        }

        public void UAccountsLockInit(Guid userId)
        {
            Mutex.Init(userContainer, U_ACCOUNTS + userId + LOCK);
        }

        public HashSetBlob<Guid> UAccountsData(Guid userId)
        {
            return new HashSetBlob<Guid>(userContainer, U_ACCOUNTS + userId + DATA);
        }

        // account Blobs

        // list blobs

        // msg blobs

        // paths
        const string DATA = "/data";
        const string LOCK = "/lock";

        const string U_INFO = "info/";
        const string U_ACCOUNTS = "accounts/";
        const string U_IDBYLOGIN = "idbylogin/";
        const string U_PASSWORD = "password/";
	}
}
