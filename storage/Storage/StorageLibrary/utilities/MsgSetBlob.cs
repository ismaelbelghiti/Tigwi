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
    }
}

