using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageCommon;

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
            if (DateTime.Now - lastMsgId < new TimeSpan(60000000000))
            {
                List<IMessage> messages = listsId.Aggregate<Guid, List<IMessage>>(new List<IMessage>(), (messagelist, list) =>
                (System.Collections.Generic.List<IMessage>)
                messagelist.Union<IMessage>(new StrgBlob<List<IMessage>>
                    (connexion.msgContainer, list.ToString()).Get()));
                messages.Sort();
                messages.Reverse();
                return messages.GetRange(0, 100);
            }
            else
            {
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
