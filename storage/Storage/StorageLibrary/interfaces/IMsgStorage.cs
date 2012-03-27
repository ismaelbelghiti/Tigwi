using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IMsgStorage
    {
        List<IMessage> GetListsMsgFrom(HashSet<int> listsId, int firstMsgId, int msgNumber);
        List<IMessage> GetListsMsgTo(HashSet<int> listsId, int lastMsgId, int msgNumber);
        
        void Tag(int accountId, int msgId);
        void Untag(int accoundId, int msgId);

        List<IMessage> GetTaggedFrom(int accoundId, int firstMsgId, int msgNumber);
        List<IMessage> GetTaggedTo(int accountId, int lastMsgId, int msgNumber);

        int Post(int accountId, string content);
        int Copy(int accountId, int msgId);
        void Remove(int id);
    }
}
