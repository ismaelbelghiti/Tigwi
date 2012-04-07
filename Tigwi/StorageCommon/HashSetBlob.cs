using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace StorageCommon
{
    public class HashSetBlob<T>
    {
        CloudBlob blob;
        BinaryFormatter formatter;

        public HashSetBlob(CloudBlobContainer container, string blobName)
        {
            blob = container.GetBlobReference(blobName);
            formatter = new BinaryFormatter();
        }

        public bool Add(T item)
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
                    eTag = blob.Attributes.Properties.ETag;
                    stream = blob.OpenRead();
                    set = (HashSet<T>)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch
                {
                    return false;
                }

                set.Add(item);
                reqOpt.AccessCondition = AccessCondition.IfMatch(eTag);

                try
                {
                    stream = blob.OpenWrite(reqOpt);
                    formatter.Serialize(stream, set);
                    stream.Close();
                    keepGoing = false;
                }
                catch(Exception)
                {
                    keepGoing = true;
                }

            } while (keepGoing);

            return true;
        }
    }
}
