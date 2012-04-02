using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageCommon
{
    public class StrgBlob<T>
    {
        BinaryFormatter formatter;
        CloudBlob blob;

        public bool Exists
        {
            get{
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


        public StrgBlob(CloudBlobContainer container, string blobName)
        {
            formatter = new BinaryFormatter();
            blob = container.GetBlobReference(blobName);
        }

        public T GetIfExists(Exception e)
        {
            // TODO : better error handling
            // TODO : replace the exception by an error code
            try
            {
                BlobStream stream = blob.OpenRead();
                T t = (T)formatter.Deserialize(stream);
                stream.Close();
                return t;
            }
            catch (Exception ee)
            {
                throw e;
            }
        }

        public T Get()
        {
            BlobStream stream = blob.OpenRead();
            T t = (T)formatter.Deserialize(stream);
            stream.Close();
            return t;
        }

        public bool SetIfExsits(object obj)
        {
            if (Exists)
            {
                BlobStream stream = blob.OpenWrite();
                formatter.Serialize(stream, obj);
                stream.Close();
                return true;
            }
            else
                return false;
        }
    }
}
