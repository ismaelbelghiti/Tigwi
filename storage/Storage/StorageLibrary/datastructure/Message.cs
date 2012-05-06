using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace StorageLibrary
{
    [ProtoContract]
    public class Message : IMessage
    {
        public Message(Guid id, Guid posterId, string PosterName, string PosterAvatar, DateTime date, string content)
        {
            Id = id;
            PosterId = posterId;
            Date = date;
            Content = content;
        }

        public Message()
        {
            Id = new Guid();
            PosterId = new Guid();
            Date = DateTime.MinValue;
            Content = null;
            PosterAvatar = null;
        }

        [ProtoMember(1)]
        public Guid Id { get; set; }

        [ProtoMember(2)]
        public Guid PosterId { get; set; }

        [ProtoMember(3)]
        public string PosterName { get; set; }

        [ProtoMember(4)]
        public string PosterAvatar { get; set; }

        [ProtoMember(5)]
        public DateTime Date { get; set; }

        [ProtoMember(6)]
        public string Content { get; set; }

        public static Message FirstMessage(DateTime date)
        {
            return new Message(new Guid(), new Guid(), "", "", date, "");
        }

        public static Message FirstMessage()
        {
            return FirstMessage(DateTime.MinValue);
        }

        public static Message LastMessage(DateTime date)
        {
            return new Message(new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid(), "", "", date, "");
        }

        public static Message LastMessage()
        {
            return Message.LastMessage(DateTime.MaxValue);
        }
    }
}
