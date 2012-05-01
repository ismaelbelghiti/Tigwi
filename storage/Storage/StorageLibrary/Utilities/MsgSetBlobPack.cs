using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary.Utilities;
using Microsoft.WindowsAzure.StorageClient;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using StorageLibrary.exception;

namespace StorageLibrary.Utilities
{
    // TODO : 
    // - maybe the guid to dectect changes is not the best fing in terms of performance
    class MsgSetBlobPack
    {
        const int splitSize = 40;
        const int mergeSize = 10;
        const string timeFormat = "yyyy-MM-dd-HH-mm-ss-fffffff";

        CloudBlobDirectory dir;

        public MsgSetBlobPack(CloudBlobContainer container, string path)
        {
            dir = container.GetDirectoryReference(path);
        }

        public void Init()
        {
            Blob<MessageSet> blob = new Blob<MessageSet>(dir.Container, dir.Uri + DateTime.MinValue.ToString(timeFormat));
            blob.AddMetadata("version", Guid.NewGuid().ToString());
            blob.Set(new MessageSet());
        }

        public List<IMessage> GetMessagesFrom(DateTime date, int msgCount, Exception e)
        {
            List<IMessage> msgList = null;

            // if their is a change in te architecture of packs while we retreive messages, then we try again
            // To detect when the structure has changed, we add a guid in metadata to get the version of the structure
            bool keepGoing = true;
            while (keepGoing)
            {
                keepGoing = false;

                // get blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();

                if (!blobsList.Any())
                    throw e;

                // get the right blob
                // TODO : find a better datasutrcture to do this faster
                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= date));

                // get the messages
                Blob<SortedSet<IMessage>> bMsgSet = new Blob<SortedSet<IMessage>>(blobsList[blobIndex].Value);
                string guid = bMsgSet.Metadata["version"];
                SortedSet<IMessage> msgSet;
                try { msgSet = bMsgSet.GetIfExists(new Exception()); }
                catch { continue; }

                // check that the version hasn't changed
                if (bMsgSet.Metadata["version"] != guid)
                    continue;

                msgList = msgSet.GetViewBetween(Message.FirstMessage(date), Message.LastMessage()).ToList();

                blobIndex++;

                // get messages from following sets while we need them
                while (msgList.Count < msgCount && blobIndex<blobsList.Count)
                {
                    bMsgSet = new Blob<SortedSet<IMessage>>(blobsList[blobIndex].Value);
                    guid = bMsgSet.Metadata["version"];
                    try { msgSet = bMsgSet.GetIfExists(new Exception()); }
                    catch { continue; }

                    // check that the version hasn't changed
                    if (bMsgSet.Metadata["version"] != guid)
                    {
                        keepGoing = true;
                        break;
                    }

                    msgList.AddRange(msgSet);
                    blobIndex++;
                }
            }

            if (msgList.Count > msgCount)
                msgList = msgList.GetRange(0, msgCount);

            return msgList;
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
                // find the blob
                // TODO : find a better datasutrcture to do this faster
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();
                if (!blobsList.Any())
                    return false;

                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= message.Date));

                // inserte data
                Blob<MessageSet> blob = new Blob<MessageSet>(blobsList[blobIndex].Value);
                MessageSet set;
                try
                {
                    do
                    {
                        set = GetMessageSet(blob);
                        set.Add(message);

                        // split if necessary
                        // TODO : should we export this to a worker
                        if (set.Count > splitSize)
                        {
                            MessageSet set1 = new MessageSet(set.Take(set.Count / 2));
                            MessageSet set2 = new MessageSet(set.Skip(set.Count / 2));

                            Blob<MessageSet> blob2 = new Blob<MessageSet>(dir.Container, dir.Uri + set2.Min.Date.ToString(timeFormat));

                            // set version to 0 to be sure no one insert/remove a message while we split
                            blob.AddMetadata("version", Guid.Empty.ToString());
                            if (!blob.TryUploadMetadata())
                                throw new VersionHasChanged();

                            // specify new versions numbers
                            blob.AddMetadata("version", Guid.NewGuid().ToString());
                            blob2.AddMetadata("version", Guid.NewGuid().ToString());

                            // upload blobs
                            blob2.Set(set2);
                            blob.Set(set1);
                            return true;
                        }

                    } while (!blob.TrySet(set));
                }
                catch (VersionHasChanged) { continue; }

                return true;
            }
        }

        public void RemoveMessage(IMessage message)
        {
            while (true)
            {
                // find the blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();
                if (!blobsList.Any())
                    return;
                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= message.Date));

                // inserte data
                Blob<MessageSet> blob = new Blob<MessageSet>(blobsList[blobIndex].Value);
                MessageSet set;
                try
                {
                    do
                    {
                        set = GetMessageSet(blob);
                        set.Remove(message);

                        // merge if necessary -- and if it is possible
                        // TODO : should we move this to the worker
                        if (set.Count < mergeSize && blobIndex < blobsList.Count-1 )
                        {
                            Blob<MessageSet> blob2 = new Blob<MessageSet>(blobsList[blobIndex + 1].Value);

                            MessageSet set2 = blob2.GetIfExists(new VersionHasChanged());

                            // set version to 0 to be sure no one insert/remove a message while we split
                            blob.AddMetadata("version", Guid.Empty.ToString());
                            if (!blob.TryUploadMetadata())
                                throw new VersionHasChanged();

                            if (!blob2.TryDelete())
                            {
                                // restore old version
                                blob.AddMetadata("version", Guid.NewGuid().ToString());
                                blob.UploadMetadata();
                                throw new VersionHasChanged();
                            }
                            // specify new versions numbers
                            blob.AddMetadata("version", Guid.NewGuid().ToString());
                            set.UnionWith(set2);
                            blob.Set(set);
                            return;
                        }

                    } while (!blob.TrySet(set));
                }
                catch (VersionHasChanged) { continue; }

                return;
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
            BlobRequestOptions requestOptions = new BlobRequestOptions();
            requestOptions.BlobListingDetails = BlobListingDetails.Metadata;

            IEnumerable<CloudBlob> blobs = dir.ListBlobs(requestOptions).OfType<CloudBlob>();
            return blobs.Select(c => new KeyValuePair<DateTime, CloudBlob>(NameToDate(c.Name), c)).ToList();
        }

        DateTime NameToDate(string blobName)
        {
            string dateString = blobName.Substring(blobName.LastIndexOf("/") + 1);
            return DateTime.ParseExact(dateString, "yyyy-MM-dd-HH-mm-ss-fffffff", CultureInfo.InvariantCulture);
        }

        MessageSet GetMessageSet(Blob<MessageSet> blob)
        {
            string guid = blob.Metadata["version"];
            MessageSet set = blob.GetIfExists(new VersionHasChanged());
            // if the guid = 0, then the architecture is changing
            if (guid != blob.Metadata["version"] || guid == Guid.Empty.ToString())
                throw new VersionHasChanged();
            return set;
        }

        class VersionHasChanged : Exception { }
    }
}
