using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageCommon
{
    public class StrgBlob<T>
    {
        // TODO : allow asynchronous upload and download
        // TODO : see if it is a good thing -> perf tests
        // TODO : if it is a good thing, use it where we can
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
            catch (Exception)
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

        public void Set(T obj)
        {
            BlobStream stream = blob.OpenWrite();
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public bool SetIfNotExists(T obj)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            reqOpt.AccessCondition = AccessCondition.IfNoneMatch("*");
            try
            {
                BlobStream stream = blob.OpenWrite(reqOpt);
                formatter.Serialize(stream, obj);
                stream.Close();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode != StorageErrorCode.ConditionFailed && e.ErrorCode != StorageErrorCode.BlobAlreadyExists)
                    throw;
                return false;
            }
        }

        // we can't use the Etag * with IfMatch 
        // beacause it only works with IfNoneMatch
        // We have to use an Etag retry policy --> TODO : find something better
        public bool SetIfExists(T obj)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            try
            {
                while (true)
                {
                    blob.FetchAttributes();
                    string etag = blob.Attributes.Properties.ETag;
                    reqOpt.AccessCondition = AccessCondition.IfMatch(etag);
                    try
                    {
                        BlobStream stream = blob.OpenWrite(reqOpt);
                        formatter.Serialize(stream, obj);
                        stream.Close();
                        return true;
                    }
                    catch (StorageClientException e)
                    {
                        if (e.ErrorCode != StorageErrorCode.ConditionFailed)
                            throw;
                    }
                }
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                    return false;
                else
                    throw;
            }
        }

        public void Delete()
        {
            blob.DeleteIfExists();
        }
    }
}
