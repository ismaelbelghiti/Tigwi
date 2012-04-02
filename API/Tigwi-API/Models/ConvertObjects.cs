using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageLibrary;
using System.Xml.Serialization;
using System.IO;

namespace Tigwi_API.Models
{

    // models for answers to GET requests

    [Serializable]
    public class MessageList
    {
        public MessageList()
        {
            Message = new List<IMessage>();
            Size = 0;
        }
        public MessageList(List<IMessage> msgs)
        {
            Message = msgs;
            Size = msgs.Count();
        }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlElement] public List<IMessage> Message;
    }

    [Serializable]
    [XmlTypeAttribute("User")]
    public class UserApi
    {
        public UserApi(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserList
    {
        public UserList()
        {
            User = new List<UserApi>();
            Size = 0;
        }
        public UserList(List<UserApi> users)
        {
            User = users;
            Size = users.Count();
        }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlElement] public List<UserApi> User;
    }

    // models to answer to POST requests

    public class Error
    {
        public Error()
        {
            Number = null;
        }
        public Error(int code)
        {
            Number = code;
        }

        public int? Number { get; set; }
    }

    // Models for request bodies

    [Serializable]
    [XmlRootAttribute("Write")]
    public class MsgToWrite
    {
        public string User { get; set; }
        public MsgToPost Message { get; set; }
    }

    [XmlTypeAttribute("Message")]
    public class MsgToPost
    {
        public string Content { get; set; }
    }

    [Serializable]
    public class Subscribe
    {
        public string User { get; set; }
        public string Subscription { get; set; }
    }

}