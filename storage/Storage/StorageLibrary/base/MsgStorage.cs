using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageCommon;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLibrary
{
    public class MsgStorage : IMsgStorage
    {
        StrgConnexion connexion;

        // Constuctor
        public MsgStorage(StrgConnexion connexion)
        {
            this.connexion = connexion;
        }

        // Interface implementation
        public List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgId, int msgNumber)
        {
            if (DateTime.Now - lastMsgId < new TimeSpan(60000000000) && msgNumber < 100)
            {
                List<IMessage> messages = listsId.Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (messagelist, list) =>
                    (System.Collections.Generic.List<IMessage>)
                messagelist.Union<IMessage>(new StrgBlob<List<IMessage>>
                    (connexion.msgContainer, Path.M_LISTMESSAGES + list).GetIfExists(new ListNotFound())));
                messages.Sort();
                messages.Reverse();
                return messages.GetRange(0, msgNumber);
            }
            else
            {
             /*   List<IMessage> messages = listsId.Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (messagelist, list) =>
                    (System.Collections.Generic.List<IMessage>)
                messagelist.Union<IMessage>(
                (new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + list + Path.L_FOLLOWEDACC_DATA)).GetIfExists(new ListNotFound()).
                Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (listmessages, account) =>
                    (System.Collections.Generic.List<IMessage>)
                listmessages.Union<IMessage>(
                connexion.msgContainer.GetDirectoryReference(account + "/").ListBlobs().OrderBy(blob => blob.Uri.AbsolutePath).
                TakeWhile(blob => (Int64.Parse(blob.Uri.AbsolutePath) > lastMsgId.ToBinary())).Aggregate<IEnumerable<IListBlobItem>,List<IMessage>>
                (new List<IMessage>(), (blobmessages,currentblob) =>
                    (System.Collections.Generic.List<IMessage>)
                    blobmessages.Union<IMessage>(new StrgBlob<List<IMessage>>(connexion.msgContainer, Path.M_ACCOUNTMESSAGES + account + "/" + currentblob.Uri.AbsolutePath).Get()))
                    ))));
                messages.Sort();
                messages.Reverse();
                return messages.GetRange(0, msgNumber);*/
                throw new NotImplementedException();
            }
        }

        public void Tag(Guid accountId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public void Untag(Guid accoundId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public Guid Post(Guid accountId, string content)
        {
            throw new NotImplementedException();
        }

        public Guid Copy(Guid accountId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
