using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Tigwi.Storage.Library.Utilities
{
    [Serializable]
    public class MessageSet : SortedSet<Message>, ISerializable
    {
        [Serializable]
        class MsgComparer : IComparer<Message>
        {
            public int Compare(Message x, Message y)
            {
                if (x.Date == y.Date)
                    return x.Id.CompareTo(y.Id);
                else
                    return x.Date < y.Date ? -1 : 1;
            }
        }

        public MessageSet() : base(new MsgComparer()) { }

        public MessageSet(SortedSet<Message> set) : base(set, new MsgComparer()) { }

        public MessageSet(IEnumerable<Message> items) : base(items, new MsgComparer()) { }

        // TODO : replace this by something more efficient
        public new MessageSet GetViewBetween(Message first, Message last)
        {
            return new MessageSet(base.GetViewBetween(first, last));
        }

        protected MessageSet(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
