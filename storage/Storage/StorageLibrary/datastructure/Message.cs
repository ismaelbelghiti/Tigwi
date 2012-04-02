using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    [Serializable]
    class Message : IMessage
    {
        public Message(Guid id, Guid posterId, DateTime date, string content)
        {
            Id = id;
            PosterId = posterId;
            Date = date;
            Content = content;
        }

        public Guid Id { get; set; }

        public Guid PosterId { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }
    }
}
