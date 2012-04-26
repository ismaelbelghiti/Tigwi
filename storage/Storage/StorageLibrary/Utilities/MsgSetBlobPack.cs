using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary.Utilities;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLibrary.Utilities
{
    class MsgSetBlobPack
    {
        CloudBlobDirectory dir;

        public MsgSetBlobPack(CloudBlobContainer container, string path)
        {
            dir = container.GetDirectoryReference(path);
        }

        public void Init()
        {
            MsgSetBlob bTaggedMsg = new MsgSetBlob(dir.Container, dir.Uri + DateTime.MinValue.ToString("yyyy-MM-dd-HH-mm-ss-fffffff"));
            bTaggedMsg.Init();
        }

        public SortedSet<IMessage> GetMessagesFrom(DateTime date, int msgCount, Exception e)
        {
            throw new NotImplementedException();
        }

        public SortedSet<IMessage> GetMessagesTo(DateTime date, int msgCount, Exception e)
        {
            throw new NotImplementedException();
        }

        // return false to warn that the message was not added
        public bool AddMessage(IMessage message)
        {
            throw new NotImplementedException();
        }

        public void RemoveMessage(IMessage message)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            // We first delete the root blob to ensure no one will be writing when we delete
            MsgSetBlob bTaggedMsg = new MsgSetBlob(dir.Container, dir.Uri + DateTime.MinValue.ToString("yyyy-MM-dd-HH-mm-ss-fffffff"));
            bTaggedMsg.Delete();

            foreach (CloudBlob b in dir.ListBlobs())
                b.Delete();
        }
    }
}
