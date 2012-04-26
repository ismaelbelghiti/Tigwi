using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLibrary.Utilities
{
    /// <summary>
    /// Base class for class used to manipulate objects in the cloud
    /// </summary>
    public class BaseBlob
    {
        protected BinaryFormatter formatter;
        protected CloudBlob blob;

        protected BaseBlob(CloudBlobContainer container, string blobName)
        {
            formatter = new BinaryFormatter();
            blob = container.GetBlobReference(blobName);
        }

        public bool Exists
        {
            get
            {
                try
                {
                    blob.FetchAttributes();
                    return true;
                }
                catch (StorageClientException e)
                {
                    if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void Delete()
        {
            blob.Delete();
        }
    }
}
