using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StorageLibrary.Utilities
{
    [Serializable]
    public class MessageSet : SortedSet<IMessage>, ISerializable
    {
        [Serializable]
        class MsgComparer : IComparer<IMessage>
        {
            public int Compare(IMessage x, IMessage y)
            {
                if (x.Date == y.Date)
                    return x.Id.CompareTo(y.Id);
                else
                    return x.Date < y.Date ? -1 : 1;
            }
        }

        public MessageSet() : base(new MsgComparer()) { }

        public MessageSet(IEnumerable<IMessage> items) : base(items, new MsgComparer()) { }

        protected MessageSet(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
