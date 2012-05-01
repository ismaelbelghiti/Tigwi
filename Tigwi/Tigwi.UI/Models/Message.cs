using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageLibrary;

namespace Tigwi.UI.Models
{
    public class Message :  IMessage
    {
        public Message(Guid id, Guid posterId, string posterName, string posterAvatar, DateTime date, string content)
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
    }
}