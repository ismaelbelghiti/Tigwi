using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageLibrary;
using System.Xml.Serialization;
using System.IO;

namespace Tigwi_API.Models
{
    [Serializable]
    public class MessageList
    {
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
        [XmlAttribute]
        public int Size { get; set; }

        [XmlElement] public List<UserApi> User;
    }

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