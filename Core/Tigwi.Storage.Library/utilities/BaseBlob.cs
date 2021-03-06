﻿#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Tigwi.Storage.Library.exception;
using System.Collections.Specialized;
using ProtoBuf;

namespace Tigwi.Storage.Library.Utilities
{
    /// <summary>
    /// Base class for class used to manipulate objects in the cloud
    /// </summary>
    public class Blob<T> where T : new()
    {
        protected CloudBlob blob;

        public Blob(CloudBlobContainer container, string blobName)
        {
            blob = container.GetBlobReference(blobName);
        }

        public Blob(CloudBlob blob)
        {
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
            MemoryStream stream;

            if (blob.Attributes.Properties.ETag == null)
                throw new EtagNotSet();
            reqOpt.AccessCondition = AccessCondition.IfMatch(blob.Attributes.Properties.ETag);

            try
            {
                using (stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    blob.UploadFromStream(stream,reqOpt);
                }
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
            T t = Deserialize(stream);
            stream.Close();
            return t;
        }
                    
        public void Set(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {   
                Serializer.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                blob.UploadFromStream(stream);
            }
        }

        public T GetIfExists(Exception e)
        {
            try
            {
                BlobStream stream = blob.OpenRead();
                T t = Deserialize(stream);
                stream.Close();
                return t;
            }
            catch(Exception)
            {
                throw e;
            }
        }

        public T GetWithDefault(T defaultValue)
        {
            try
            {
                return this.Get();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public bool SetIfNotExists(T obj)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            reqOpt.AccessCondition = AccessCondition.IfNoneMatch("*");
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    blob.UploadFromStream(stream, reqOpt);
                }
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
                        using (MemoryStream stream = new MemoryStream())
                        {
                            Serializer.Serialize(stream, obj);
                            stream.Seek(0, SeekOrigin.Begin);
                            blob.UploadFromStream(stream, reqOpt);
                        }
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

        T Deserialize(Stream stream)
        {
            T t = Serializer.Deserialize<T>(stream);
            if (t == null)
                t = new T();
            return t;
        }
    }
}
