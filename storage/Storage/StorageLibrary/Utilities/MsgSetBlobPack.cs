using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary.Utilities;
using Microsoft.WindowsAzure.StorageClient;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;

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

        public List<IMessage> GetMessagesFrom(DateTime date, int msgCount, Exception e)
        {
            // if their is a change in te architecture of packs while we retreive messages, then we try again
            // TODO : how to detect when something has changed ?
            // - add a GUI in the name to see when we change
            // - add a GUI in metadata
            // - the GUI would only change if we change the structure
            while (true)
            {
                // get blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();

                if (!blobsList.Any())
                    throw e;

                // get the right blob
                // TODO : find a better datasutrcture to do this faster
                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= date));

                // get the messages
                Blob<SortedSet<IMessage>> bMsgSet = new Blob<SortedSet<IMessage>>(blobsList[blobIndex].Value);
                SortedSet<IMessage> msgSet;
                try { msgSet = bMsgSet.GetIfExists(new Exception()); }
                catch { continue; }

                List<IMessage> msgList = msgSet.GetViewBetween(Message.FirstMessage(date), Message.LastMessage()).ToList();

                blobIndex++;

                // get messages from following sets while we need them
                while (msgList.Count < msgCount && blobIndex<blobsList.Count)
                {
                    bMsgSet = new Blob<SortedSet<IMessage>>(blobsList[blobIndex].Value);
                    try { msgSet = bMsgSet.GetIfExists(new Exception()); }
                    catch { continue; }

                    msgList.AddRange(msgSet);
                    blobIndex++;
                }

                if (msgList.Count > msgCount)
                    msgList = msgList.GetRange(0, msgCount);

                return msgList;
            }
        }

        // NYI
        public List<IMessage> GetMessagesTo(DateTime date, int msgCount, Exception e)
        {
            // get blobs
            List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();

            throw new NotImplementedException();

            // find the right blob

            // get the message

            // get the message in the nexts blob while it is possible and necessary
        }

        // return false to warn that the message was not added
        public bool AddMessage(IMessage message)
        {
            while (true)
            {
                // get blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();

                if (!blobsList.Any())
                    return false;

                // get the right blob
                // TODO : find a better datasutrcture to do this faster
                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= message.Date));

                // inserte data
                CloudBlob blob = blobsList[blobIndex].Value;
                SortedSet<IMessage> msgSet;
                BlobRequestOptions reqOpt = new BlobRequestOptions();
                BlobStream stream;
                string eTag;
                BinaryFormatter formatter = new BinaryFormatter();

                while(true)
                {
                    try
                    {
                        blob.FetchAttributes();
                        eTag = blob.Attributes.Properties.ETag;
                        stream = blob.OpenRead();
                        msgSet = (SortedSet<IMessage>)formatter.Deserialize(stream);
                        stream.Close();
                    }
                    catch { break; }

                    // TODO : split if necessary

                    msgSet.Add(message);
                    reqOpt.AccessCondition = AccessCondition.IfMatch(eTag);

                    try
                    {
                        stream = blob.OpenWrite(reqOpt);
                        formatter.Serialize(stream, msgSet);
                        stream.Close();
                        return true;
                    }
                    catch { }
                }
            }
        }

        // NYI
        public void RemoveMessage(IMessage message)
        {
            while (true)
            {
                // get blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();

                throw new NotImplementedException();

                // find the good blob

                // remove data

                // merge if necessary

                // try to save
            }
        }

        public void Delete()
        {
            foreach (CloudBlob b in dir.ListBlobs())
                b.Delete();
        }

        List<KeyValuePair<DateTime, CloudBlob>> GetBlobs()
        {
            // Blobs are already ordered since they are retrieve by alphabetical order
            // TODO : Check this assertion
            IEnumerable<CloudBlob> blobs = dir.ListBlobs().OfType<CloudBlob>();
            return blobs.Select(c => new KeyValuePair<DateTime, CloudBlob>(NameToDate(c.Name), c)).ToList();
        }

        DateTime NameToDate(string blobName)
        {
            string dateString = blobName.Substring(blobName.LastIndexOf("/") + 1);
            return DateTime.ParseExact(dateString, "yyyy-MM-dd-HH-mm-ss-fffffff", CultureInfo.InvariantCulture);
        }
    }
}
