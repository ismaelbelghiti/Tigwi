﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using StorageLibrary.exception;
using System.Collections.Specialized;

namespace StorageLibrary.Utilities
{
    /// <summary>
    /// Base class for class used to manipulate objects in the cloud
    /// </summary>
    public class Blob<T>
    {
        protected BinaryFormatter formatter;
        protected CloudBlob blob;

        public Blob(CloudBlobContainer container, string blobName)
        {
            formatter = new BinaryFormatter();
            blob = container.GetBlobReference(blobName);
        }

        public Blob(CloudBlob blob)
        {
            formatter = new BinaryFormatter();
            this.blob = blob;
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

        public bool TrySet(T obj)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            BlobStream stream;

            if (blob.Attributes.Properties.ETag == null)
                throw new EtagNotSet();
            reqOpt.AccessCondition = AccessCondition.IfMatch(blob.Attributes.Properties.ETag);

            try
            {
                stream = blob.OpenWrite(reqOpt);
                formatter.Serialize(stream, obj);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
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

        public T GetIfExists(Exception e)
        {
            try
            {
                BlobStream stream = blob.OpenRead();
                T t = (T)formatter.Deserialize(stream);
                stream.Close();
                return t;
            }
            catch(Exception)
            {
                throw e;
            }
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
            blob.Delete();
        }

        public bool TryDelete()
        {
            if (blob.Attributes.Properties.ETag == null)
                throw new EtagNotSet();

            BlobRequestOptions reqOpt = new BlobRequestOptions();
            reqOpt.AccessCondition = AccessCondition.IfMatch(blob.Attributes.Properties.ETag);

            try
            {
                blob.Delete(reqOpt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddMetadata(string name, string value)
        {
            blob.Metadata.Add(name, value);
        }

        public void UploadMetadata()
        {
            blob.SetMetadata();
        }

        public bool TryUploadMetadata()
        {
            if (blob.Attributes.Properties.ETag == null)
                throw new EtagNotSet();

            BlobRequestOptions reqOpt = new BlobRequestOptions();
            reqOpt.AccessCondition = AccessCondition.IfMatch(blob.Attributes.Properties.ETag);

            try
            {
                blob.SetMetadata(reqOpt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public NameValueCollection Metadata
        {
            get
            {
                return blob.Metadata;
            }
        }
    }
}