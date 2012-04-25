using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace StorageLibrary.Utilities
{
    public class MsgSetBlob
    {
        /*CloudBlob blob;
        BinaryFormatter formatter;

        public MsgSetBlob(CloudBlobContainer container, string blobName)
        {
            blob = container.GetBlobReference(blobName);
            formatter = new BinaryFormatter();
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

        public bool Add(MessageUpdateFields )
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            SortedList<K, V> list;
            BlobStream stream;
            string eTag;
            bool keepGoing;

            do
            {
                try
                {
                    blob.FetchAttributes();
                    eTag = blob.Attributes.Properties.ETag;
                    stream = blob.OpenRead();
                    list = (SortedList<K, V>)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch
                {
                    return false;
                }

                list.Add(key, item);
                reqOpt.AccessCondition = AccessCondition.IfMatch(eTag);

                try
                {
                    stream = blob.OpenWrite(reqOpt);
                    formatter.Serialize(stream, list);
                    stream.Close();
                    keepGoing = false;
                }
                catch (Exception)
                {
                    keepGoing = true;
                }

            } while (keepGoing);

            return true;
        }

        public bool Remove(T item)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            HashSet<T> set;
            BlobStream stream;
            string eTag;
            bool keepGoing;

            do
            {
                try
                {
                    blob.FetchAttributes();
                    eTag = blob.Attributes.Properties.ETag;
                    stream = blob.OpenRead();
                    set = (HashSet<T>)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch
                {
                    return false;
                }

                set.Remove(item);
                reqOpt.AccessCondition = AccessCondition.IfMatch(eTag);

                try
                {
                    stream = blob.OpenWrite(reqOpt);
                    formatter.Serialize(stream, set);
                    stream.Close();
                    keepGoing = false;
                }
                catch (Exception)
                {
                    keepGoing = true;
                }

            } while (keepGoing);

            return true;
        }*/
    }
}

