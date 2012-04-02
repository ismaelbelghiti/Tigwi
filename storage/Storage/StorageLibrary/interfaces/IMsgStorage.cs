using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IMsgStorage
    {
        // TODO : specify the exceptions that each methodes can throw
        List<IMessage> GetListsMsgFrom(HashSet<Guid> listsId, DateTime firstMsgId, int msgNumber);
        List<IMessage> GetListsMsgTo(HashSet<Guid> listsId, DateTime lastMsgId, int msgNumber);
        
        void Tag(Guid accountId, Guid msgId);
        void Untag(Guid accoundId, Guid msgId);

        List<IMessage> GetTaggedFrom(Guid accoundId, DateTime firstMsgId, int msgNumber);
        List<IMessage> GetTaggedTo(Guid accountId, DateTime lastMsgId, int msgNumber);

        int Post(Guid accountId, string content);
        int Copy(Guid accountId, Guid msgId);
        void Remove(Guid id);
    }
}
