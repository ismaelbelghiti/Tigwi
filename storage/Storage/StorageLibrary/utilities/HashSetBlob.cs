using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace StorageLibrary.Utilities
{
    public class HashSetBlob<T> : Blob<HashSet<T>>
    {
        public HashSetBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public bool Add(T item)
        {
            HashSet<T> set;

            do
            {
                try
                {
                    BlobStream stream = blob.OpenRead();
                    set = (HashSet<T>)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch { return false; }

                set.Add(item);
                

            } while (!base.TrySet(set));

            return true;
        }

        public bool Remove(T item)
        {
            HashSet<T> set;

            do
            {
                try
                {
                    BlobStream stream = blob.OpenRead();
                    set = (HashSet<T>)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch { return false; }

                set.Remove(item);

            } while (!base.TrySet(set));

            return true;
        }
    }
}
