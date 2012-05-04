using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary.Utilities;
using Microsoft.WindowsAzure.StorageClient;
using System.Globalization;
using StorageLibrary.exception;

namespace StorageLibrary.Utilities
{
    // TODO : maybe the guid to dectect changes is not the best fing in terms of performance
    // TODO : change to private
    public class MsgSetBlobPack
    {
        const int splitSize = 200;
        const int mergeSize = 50;
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

        // TODO : change list to set for a better merge
        public List<IMessage> GetMessagesFrom(DateTime date, int msgCount, Exception e)
        {
            MessageSet msgSet = null;
            // if their is a change in te architecture of packs while we retreive messages, then we try again
            // To detect when the structure has changed, we add a guid in metadata to get the version of the structure
            do
            {
                // get blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();
                if (!blobsList.Any())
                    throw e;

                // get the right blob
                // TODO : find a better datasutrcture to do this faster
                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= date));

                // get the messages
                try
                {
                    msgSet = GetMessageSet(new Blob<MessageSet>(blobsList[blobIndex].Value));
                    msgSet = msgSet.GetViewBetween(Message.FirstMessage(date), Message.LastMessage());

                    // get messages from following sets while we need them
                    for(blobIndex++; blobIndex<blobsList.Count && msgSet.Count < msgCount; blobIndex++)
                        msgSet.UnionWith(GetMessageSet(new Blob<MessageSet>(blobsList[blobIndex].Value)));
                }
                catch (VersionHasChanged) { continue; }

            } while (false);

            List<IMessage> msgList = msgSet.ToList();
            if (msgList.Count > msgCount)
                msgList = msgList.GetRange(0, msgCount);

            return msgList;
        }

        // TODO : change list to set for a better merge
        public List<IMessage> GetMessagesTo(DateTime date, int msgCount, Exception e)
        {
            MessageSet msgSet = null;

            // TODO : use something else than reverse
            do
            {
                // get blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();
                if (!blobsList.Any())
                    throw e;

                // get the right blob
                // TODO : find a better datasutrcture to do this faster
                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= date));

                // get the messages
                try
                {
                    msgSet = GetMessageSet(new Blob<MessageSet>(blobsList[blobIndex].Value));
                    msgSet = msgSet.GetViewBetween(Message.FirstMessage() , Message.LastMessage(date));

                    // get messages from following sets while we need them
                    for (blobIndex--; blobIndex >= 0 && msgSet.Count < msgCount; blobIndex--)
                        msgSet.UnionWith(GetMessageSet(new Blob<MessageSet>(blobsList[blobIndex].Value)));                     
                }
                catch (VersionHasChanged) { continue; }

               
            } while (false);

            List<IMessage> msgList = msgSet.ToList();
            if (msgList.Count > msgCount)
                msgList = msgList.GetRange(msgList.Count - msgCount, msgCount);

            return msgList;
        }

        public MessageSet GetMessagesBetween(DateTime first, DateTime last)
        {
            MessageSet msgSet = null;
            // if their is a change in te architecture of packs while we retreive messages, then we try again
            // To detect when the structure has changed, we add a guid in metadata to get the version of the structure
            do
            {
                // get blobs
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();
                if (!blobsList.Any())
                    return new MessageSet();

                // get the right blob
                // TODO : find a better datasutrcture to do this faster
                int blobIndex = blobsList.IndexOf(blobsList.Last(p => p.Key <= first));

                // get the messages
                try
                {
                    msgSet = GetMessageSet(new Blob<MessageSet>(blobsList[blobIndex].Value));

                    // get messages from following sets while we need them
                    for (blobIndex++; blobIndex < blobsList.Count && msgSet.Max.Date < last; blobIndex++ )
                        msgSet.UnionWith(GetMessageSet(new Blob<MessageSet>(blobsList[blobIndex].Value)));
                }
                catch (VersionHasChanged) { continue; }

            } while (false);
           
            return msgSet.GetViewBetween(Message.FirstMessage(first), Message.LastMessage(last));
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

        public bool UnionWith(MsgSetBlobPack other)
        {
            DateTime progress = DateTime.MinValue;

            while (progress != DateTime.MaxValue)
            {
                List<KeyValuePair<DateTime, CloudBlob>> blobsList = GetBlobs();
                if (!blobsList.Any())
                    return false;
                try
                {
                    for (int i = 0; i < blobsList.Count; i++)
                    {
                        DateTime upperBound = i == blobsList.Count ? DateTime.MaxValue : blobsList[i + 1].Key;
                        if (upperBound < progress)
                            continue;

                        MessageSet set = GetMessageSet(new Blob<MessageSet>(blobsList[i].Value));
                        set.UnionWith(other.GetMessagesBetween(blobsList[i].Key, upperBound));
                        progress = upperBound;
                    }
                }
                catch (VersionHasChanged) { continue; }
            }

            return true;
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
