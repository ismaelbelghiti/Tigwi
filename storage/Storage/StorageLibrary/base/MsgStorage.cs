using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public class MsgStorage : IMsgStorage
    {
        Storage storageAcces;

        // Constuctor
        public MsgStorage(Storage storageAcces)
        {
            this.storageAcces = storageAcces;
        }

        // Interface implementation
        public List<IMessage> GetListsMsgFrom(HashSet<int> listsId, int firstMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetListsMsgTo(HashSet<int> listsId, int lastMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public void Tag(int accountId, int msgId)
        {
            throw new NotImplementedException();
        }

        public void Untag(int accoundId, int msgId)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetTaggedFrom(int accoundId, int firstMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public List<IMessage> GetTaggedTo(int accountId, int lastMsgId, int msgNumber)
        {
            throw new NotImplementedException();
        }

        public int Post(int accountId, string content)
        {
            throw new NotImplementedException();
        }

        public int Copy(int accountId, int msgId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
