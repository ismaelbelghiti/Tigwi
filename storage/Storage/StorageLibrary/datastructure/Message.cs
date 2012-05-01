using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    [Serializable]
    class Message : IMessage
    {
        public Message(Guid id, Guid posterId, string PosterName, string PosterAvatar, DateTime date, string content)
        {
            Id = id;
            PosterId = posterId;
            Date = date;
            Content = content;
        }

        public Guid Id { get; set; }

        public Guid PosterId { get; set; }

        public string PosterName { get; set; }

        public string PosterAvatar { get; set; }

        public DateTime Date { get; set; }

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
