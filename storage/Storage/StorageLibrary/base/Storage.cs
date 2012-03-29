using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLibrary
{
    public class Storage : IStorage
    {
        // Children declaration
        IUserStorage user;
        public IUserStorage User { get { return user; } }

        IAccountStorage account;
        public IAccountStorage Account { get { return account; } }
        
        IListStorage list;
        public IListStorage List { get { return list; } }

        IMsgStorage msg;
        public IMsgStorage Msg { get { return msg; } }

        // Initialisation
        public Storage(string azureAccountName, string azureKey)
        {
            // allocate childrens
            user = new UserStorage(this);
            account = new AccountStorage(this);
            list = new ListStorage(this);
            msg = new MsgStorage(this);

            SetUpStorageClient(azureAccountName, azureKey);
        }

        // Cloud Connection
        public CloudBlobContainer userContainer;
        public CloudBlobContainer accountContainer;
        public CloudBlobContainer listContainer;
        public CloudBlobContainer msgContainer;

        public CloudQueue mainQueue;

        void SetUpStorageClient(string azureAccountName, string azureKey)
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

            userContainer.CreateIfNotExist();
            accountContainer.CreateIfNotExist();
            listContainer.CreateIfNotExist();
            userContainer.CreateIfNotExist();

            // Create queue
            CloudQueueClient queueClient = azureAccount.CreateCloudQueueClient();

            mainQueue = queueClient.GetQueueReference("mainQueue");
            mainQueue.CreateIfNotExist();
        }
    }
}
