using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace StorageLibrary.Utilities
{
    public class MsgSetBlob : Blob<SortedSet<IMessage>>
    {
        [Serializable]
        class MsgComparer : IComparer<IMessage>
        {
            public int Compare(IMessage x, IMessage y)
            {
                if (x.Date == y.Date)
                    return x.Id.CompareTo(y.Id);
                else
                    return x.Date < y.Date ? -1 : 1;
            }
        }

        public MsgSetBlob(CloudBlob blob) : base(blob) { }

        public MsgSetBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public void Init()
        {
            MsgComparer comparer = new MsgComparer();
            BlobStream stream = blob.OpenWrite();
            formatter.Serialize(stream, new SortedSet<IMessage>(comparer));
            stream.Close();
        }

        /// <summary>
        /// Add a message to the set and delete the older message if count > maxMessage
        /// </summary>
        /// <returns>return false if no message was added</returns>
        public bool AddAndDelete(IMessage message, int maxMsg)
        {
            BlobRequestOptions reqOpt = new BlobRequestOptions();
            SortedSet<IMessage> set;
            BlobStream stream;
            string eTag;

            do
            {
                try
                {
                    blob.FetchAttributes();
                    eTag = blob.Attributes.Properties.ETag;
                    stream = blob.OpenRead();
                    set = (SortedSet<IMessage>)formatter.Deserialize(stream);
                    stream.Close();
                }
                catch { return false; }

                // update the set
                set.Add(message);
                // we can do this this way because we usualy remove only one
                while (set.Count > maxMsg)
                    set.Remove(set.Min);

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

