using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using ProtoBuf;

namespace StorageLibrary.Utilities
{
    // TODO : inheritance of blob is not the best thing
    // we should have a blob field instead
    // TODO : Do we realy need this class ? -- or to have it this way ?
    public class MsgSetBlob : Blob<MessageSet>
    {
        public MsgSetBlob(CloudBlob blob) : base(blob) { }

        public MsgSetBlob(CloudBlobContainer container, string blobName) : base(container, blobName) { }

        public void Init()
        {
            BlobStream stream = blob.OpenWrite();
            Serializer.Serialize(stream, new MessageSet());
            stream.Close();
        }

        /// <summary>
        /// Add a message to the set and delete the older message if count > maxMessage
        /// </summary>
        /// <returns>return false if no message was added</returns>
        public bool AddAndDelete(IMessage message, int maxMsg)
        {
            MessageSet set;
            BlobStream stream;

            do
            {
                try
                {
                    stream = blob.OpenRead();
                    set = Serializer.Deserialize<MessageSet>(stream);
                    stream.Close();
                }
                catch { return false; }

                // update the set
                set.Add(message);
                // we can do this this way because we usualy remove only one
                while (set.Count > maxMsg)
                    set.Remove(set.Min);

            } while (!base.TrySet(set));

            return true;
        }
    }
}

