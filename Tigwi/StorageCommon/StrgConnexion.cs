using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageCommon
{
    public class StrgConnexion
    {
        // Cloud Connection
        public CloudBlobContainer userContainer;
        public CloudBlobContainer accountContainer;
        public CloudBlobContainer listContainer;
        public CloudBlobContainer msgContainer;

        public CloudQueue mainQueue;

        public StrgConnexion(string azureAccountName, string azureKey)
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
            msgContainer.CreateIfNotExist();

            // Create queue
            CloudQueueClient queueClient = azureAccount.CreateCloudQueueClient();

            mainQueue = queueClient.GetQueueReference("mainqueue");
            mainQueue.CreateIfNotExist();
        }
    }
}
