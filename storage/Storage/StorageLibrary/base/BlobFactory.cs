using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

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
	}
}
