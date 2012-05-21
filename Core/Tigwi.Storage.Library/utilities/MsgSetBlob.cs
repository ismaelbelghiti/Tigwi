using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using ProtoBuf;

namespace Tigwi.Storage.Library.Utilities
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
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, new MessageSet());
                stream.Seek(0, SeekOrigin.Begin);
                blob.BeginUploadFromStream(stream, blob.EndUploadFromStream, null);
            }
        }
    }
}

