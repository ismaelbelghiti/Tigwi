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
        const TimeSpan limitDateDiff = TimeSpan.FromSeconds(5);

        StrgConnexion connexion;

        public MsgStorage(StrgConnexion connexion)
        {
            this.connexion = connexion;
        }

        // partialy implemented
        public List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgTime, int msgNumber)
        {
            // TODO : add some parallization
            SortedSet<IMessage> messages = listsId.Aggregate(new SortedSet<IMessage>(),
                (set, id) => new Blob<SortedSet<IMessage>>(connexion.msgContainer, Path.M_LISTMESSAGES + id).GetIfExists(new ListNotFound()));

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
            //    messagelist.Union<IMessage>(new Blob<List<IMessage>>(connexion.msgContainer, Path.M_LISTMESSAGES + list).GetIfExists(new ListNotFound())));
            //    messages.Sort();
            //    messages.Reverse();
            //    return messages.GetRange(0, msgNumber);
            //}
            //else
            //{
            // /*   List<IMessage> messages = listsId.Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (messagelist, list) =>
            //        (System.Collections.Generic.List<IMessage>)
            //    messagelist.Union<IMessage>(
            //    (new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDACCOUNTS + list + Path.L_FOLLOWEDACC_DATA)).GetIfExists(new ListNotFound()).
            //    Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (listmessages, account) =>
            //        (System.Collections.Generic.List<IMessage>)
            //    listmessages.Union<IMessage>(
            //    connexion.msgContainer.GetDirectoryReference(account + "/").ListBlobs().OrderBy(blob => blob.Uri.AbsolutePath).
            //    TakeWhile(blob => (Int64.Parse(blob.Uri.AbsolutePath) > lastMsgId.ToBinary())).Aggregate<IEnumerable<IListBlobItem>,List<IMessage>>
            //    (new List<IMessage>(), (blobmessages,currentblob) =>
            //        (System.Collections.Generic.List<IMessage>)
            //        blobmessages.Union<IMessage>(new Blob<List<IMessage>>(connexion.msgContainer, Path.M_ACCOUNTMESSAGES + account + "/" + currentblob.Uri.AbsolutePath).Get()))
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
            IMessage message = (new Blob<IMessage>(connexion.msgContainer, Path.M_MESSAGE + msgId)).GetIfExists(new MessageNotFound());

            // Tag it
            MsgSetBlobPack msgsBlob = new MsgSetBlobPack(connexion.msgContainer, Path.M_TAGGEDMESSAGES + accountId);
            if (!msgsBlob.AddMessage(message))
                throw new AccountNotFound();
        }

        public void Untag(Guid accountId, Guid msgId)
        {
            // retrive the message to get its date
            IMessage message;
            try
            {
                message = (new Blob<IMessage>(connexion.msgContainer, Path.M_MESSAGE + msgId)).GetIfExists(new MessageNotFound());
            }
            catch { return; }

            // remove the message from tagged
            MsgSetBlobPack msgsBlob = new MsgSetBlobPack(connexion.msgContainer, Path.M_TAGGEDMESSAGES + accountId);
            msgsBlob.RemoveMessage(message);
        }

        public List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgDate, int msgNumber)
        {
            MsgSetBlobPack bMessages = new MsgSetBlobPack(connexion.msgContainer, Path.M_TAGGEDMESSAGES + accoundId);
            List<IMessage> msgs = bMessages.GetMessagesFrom(firstMsgDate, msgNumber, new AccountNotFound());

            return TruncateMessages(msgs);
        }

        public List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgDate, int msgNumber)
        {
            MsgSetBlobPack bMessages = new MsgSetBlobPack(connexion.msgContainer, Path.M_TAGGEDMESSAGES + accountId);
            return bMessages.GetMessagesTo( TruncateDate(lastMsgDate), msgNumber, new AccountNotFound());
        }

        // partialy implemented
        public Guid Post(Guid accountId, string content)
        {
            Guid id = Guid.NewGuid();

            Blob<IMessage> bMessage = new Blob<IMessage>(connexion.msgContainer, Path.M_MESSAGE + id);
            try
            {
                Blob<AccountInfo> bAccountInfo = new Blob<AccountInfo>(connexion.accountContainer, Path.A_INFO + accountId);
                AccountInfo accountInfo = bAccountInfo.GetIfExists(new AccountNotFound());

                Message message = new Message(id, accountId, accountInfo.Name, "", DateTime.Now, content);

                // Save the message
                bMessage.Set(message);

                // Add in listMsg
                Blob<HashSet<Guid>> bFollowedBy = new Blob<HashSet<Guid>>(connexion.listContainer, Path.L_FOLLOWEDBY + accountId);

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

        DateTime TruncateDate(DateTime d)
        {
            DateTime limitDate = DateTime.Now - limitDateDiff;
            return d < limitDate ? d : limitDate;
        }

        List<IMessage> TruncateMessages(List<IMessage> msgs)
        {
            // we remove msg to recents because their is no time synchronisation between azure VMs
            DateTime dateLimit = DateTime.Now - limitDateDiff;
            return msgs.TakeWhile(m => m.Date < dateLimit).ToList();    // TODO : improve performance
        }
    }
}
