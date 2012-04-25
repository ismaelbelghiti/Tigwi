using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace StorageLibrary.Utilities
{
    public class HashSetBlob<T> : BaseBlob
    {
        public HashSetBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public bool Add(T item)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            HashSet<T> set;
            BlobStream stream;
            string eTag;

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
                catch { return false; }

                set.Add(item);
                reqOpt.AccessCondition = AccessCondition.IfMatch(eTag);

                try
                {
                    stream = blob.OpenWrite(reqOpt);
                    formatter.Serialize(stream, set);
                    stream.Close();
                    return true;
                }
                catch { }

            } while (true);
        }

        public bool Remove(T item)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            HashSet<T> set;
            BlobStream stream;
            string eTag;

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
                catch { return false; }

                set.Remove(item);
                reqOpt.AccessCondition = AccessCondition.IfMatch(eTag);

                try
                {
                    stream = blob.OpenWrite(reqOpt);
                    formatter.Serialize(stream, set);
                    stream.Close();
                    return true;
                }
                catch { }

            } while (true);
        }
    }
}
