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
        TimeSpan limitDateDiff = TimeSpan.FromSeconds(5);

        StrgConnexion connexion; // TODO : to be removed
        BlobFactory blobFactory;

        public MsgStorage(StrgConnexion connexion, BlobFactory blobFactory)
        {
            this.connexion = connexion;
            this.blobFactory = blobFactory;
        }

        public List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgTime, int msgNumber)
        {
            // TODO : add some parallization
            MessageSet messages = new MessageSet();
            foreach (Guid id in listsId)
            {
                MsgSetBlobPack blob = new MsgSetBlobPack(connexion.msgContainer, Path.M_LISTMESSAGES + id);
                messages.UnionWith(blob.GetMessagesFrom(firstMsgTime, msgNumber, new ListNotFound()));
            }

            List<IMessage> msgList = messages.ToList();
            if (msgList.Count > msgNumber)
                msgList.GetRange(0, msgNumber);

            return TruncateMessages(msgList);
        }

        public List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgTime, int msgNumber)
        {
            // TODO : add some parallization
            MessageSet messages = new MessageSet();
            lastMsgTime = TruncateDate(lastMsgTime);
            foreach (Guid id in listsId)
            {
                MsgSetBlobPack blob = new MsgSetBlobPack(connexion.msgContainer, Path.M_LISTMESSAGES + id);
                messages.UnionWith(blob.GetMessagesTo( lastMsgTime, msgNumber, new ListNotFound()));
            }

            List<IMessage> msgList = messages.ToList();
            if (msgList.Count > msgNumber)
                msgList.GetRange(0, msgNumber);

            return TruncateMessages(msgList);
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
        // TODO : update to reflect changes in the architecture
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
                // TODO : handle the fact that followedBy might change during this process
                Blob<HashSet<Guid>> bFollowedByAll = new Blob<HashSet<Guid>>(connexion.listContainer, Path.LFollowedByAll(accountId));
                HashSet<Guid> lists = bFollowedByAll.GetIfExists(new AccountNotFound());
                foreach(Guid listId in lists) 
                {
                    MsgSetBlobPack msgSet = new MsgSetBlobPack(connexion.msgContainer, Path.M_LISTMESSAGES + listId);
                    msgSet.AddMessage(message);
                }
            }
            catch { bMessage.Delete(); }
            
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
