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
            throw new NotImplementedException();
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

        public int Post(Guid accountId, string content)
        {
            throw new NotImplementedException();
        }

        public int Copy(Guid accountId, Guid msgId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
