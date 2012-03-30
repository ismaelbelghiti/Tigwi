using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    [Serializable]
    class Message : IMessage
    {
        public Message(int id, int posterId, DateTime date, string content)
        {
            Id = id;
            PosterId = posterId;
            Date = date;
            Content = content;
        }

        public int Id { get; set; }

        public int PosterId { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }
    }
}
