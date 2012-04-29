using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary;
using StorageLibrary.Utilities;
using Microsoft.WindowsAzure.StorageClient;
using System.Threading;

namespace StorageLibrary
{
    public class MsgStorage : IMsgStorage
    {
        // TODO : find the best value
        // We split the set of messages in packs because we don't want to retrive a 10M blob from the storage
        const int msgPackSize = 100;

        StrgConnexion connexion;

        public MsgStorage(StrgConnexion connexion)
        {
            this.connexion = connexion;
        }

        public List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgTime, int msgNumber)
        {
            // TODO : add some parallization
            SortedSet<IMessage> messages = listsId.Aggregate(new SortedSet<IMessage>(),
                (set, id) => new StrgBlob<SortedSet<IMessage>>(connexion.msgContainer, Path.M_LISTMESSAGES + id).GetIfExists(new ListNotFound()));

            // TODO : take into account previous messages

            List<IMessage> msgList = messages.GetViewBetween(Message.FirstMessage(firstMsgTime), Message.LastMessage()).ToList();
            if (msgList.Count > msgNumber)
                msgList.GetRange(0, msgNumber);

            return msgList;
        }

        // NYI
        public List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgTime, int msgNumber)
        {

            throw new NotImplementedException();
            //if (DateTime.Now - lastMsgTime < new TimeSpan(60000000000) && msgNumber < 100)
            //{
            //    List<IMessage> messages = listsId.Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (messagelist, list) =>
            //        (System.Collections.Generic.List<IMessage>)
            //    messagelist.Union<IMessage>(new StrgBlob<List<IMessage>>(connexion.msgContainer, Path.M_LISTMESSAGES + list).GetIfExists(new ListNotFound())));
            //    messages.Sort();
            //    messages.Reverse();
            //    return messages.GetRange(0, msgNumber);
            //}
            //else
            //{
            // /*   List<IMessage> messages = listsId.Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (messagelist, list) =>
            //        (System.Collections.Generic.List<IMessage>)
            //    messagelist.Union<IMessage>(
            //    (new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + list + Path.L_FOLLOWEDACC_DATA)).GetIfExists(new ListNotFound()).
            //    Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (listmessages, account) =>
            //        (System.Collections.Generic.List<IMessage>)
            //    listmessages.Union<IMessage>(
            //    connexion.msgContainer.GetDirectoryReference(account + "/").ListBlobs().OrderBy(blob => blob.Uri.AbsolutePath).
            //    TakeWhile(blob => (Int64.Parse(blob.Uri.AbsolutePath) > lastMsgId.ToBinary())).Aggregate<IEnumerable<IListBlobItem>,List<IMessage>>
            //    (new List<IMessage>(), (blobmessages,currentblob) =>
            //        (System.Collections.Generic.List<IMessage>)
            //        blobmessages.Union<IMessage>(new StrgBlob<List<IMessage>>(connexion.msgContainer, Path.M_ACCOUNTMESSAGES + account + "/" + currentblob.Uri.AbsolutePath).Get()))
            //        ))));
            //    messages.Sort();
            //    messages.Reverse();
            //    return messages.GetRange(0, msgNumber);*/
            //    throw new NotImplementedException();
            //}
        }

        public void Tag(Guid accountId, Guid msgId)
        {
            // retrive the message
            IMessage message = (new StrgBlob<IMessage>(connexion.msgContainer, Path.M_MESSAGE + msgId)).GetIfExists(new MessageNotFound());

            throw new NotImplementedException();
        }

        // NYI
        public void Untag(Guid accoundId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgDate, int msgNumber)
        {
            MsgSetBlobPack bMessages = new MsgSetBlobPack(connexion.msgContainer, Path.M_TAGGEDMESSAGES + accoundId);
            return bMessages.GetMessagesFrom(firstMsgDate, msgNumber, new AccountNotFound());
        }

        // NYI
        public List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public Guid Post(Guid accountId, string content)
        {
            Guid id = Guid.NewGuid();

            StrgBlob<IMessage> bMessage = new StrgBlob<IMessage>(connexion.msgContainer, Path.M_MESSAGE + id);
            try
            {
                StrgBlob<AccountInfo> bAccountInfo = new StrgBlob<AccountInfo>(connexion.accountContainer, Path.A_INFO + accountId);
                AccountInfo accountInfo = bAccountInfo.GetIfExists(new AccountNotFound());

                Message message = new Message(id, accountId, accountInfo.Name, "", DateTime.Now, content);

                // Save the message
                bMessage.Set(message);

                // Add in listMsg
                StrgBlob<HashSet<Guid>> bFollowedBy = new StrgBlob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDBY + accountId);

                foreach(Guid listId in bFollowedBy.GetIfExists(new AccountNotFound()))
                {
                    MsgSetBlob msgSet = new MsgSetBlob(connexion.msgContainer, Path.M_LISTMESSAGES + listId);
                    msgSet.AddAndDelete(message, msgPackSize);
                }
            }
            catch 
            {
                // Remove the message
                bMessage.Delete();
            }
            
            // TODO : Add in accountMsg

            return id;
        }

        // NYI
        public Guid Copy(Guid accountId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        // NYI
        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
