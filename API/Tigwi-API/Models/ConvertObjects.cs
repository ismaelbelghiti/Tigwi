using System;
using System.Collections.Generic;
using System.Linq;
using StorageLibrary;
using System.Xml.Serialization;

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
    [XmlTypeAttribute("Account")]
    public class Account
    {
        public Account(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class AccountList
    {
        public AccountList()
        {
            Account = new List<Account>();
            Size = 0;
        }
        public AccountList(List<Account> accounts)
        {
            Account = accounts;
            Size = accounts.Count();
        }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlElement] public List<Account> Account;
    }

    // models to answer to POST requests

    public class Error
    {
        public Error()
        {
            Code = null;
        }
        public Error(string code)
        {
            Code = code;
        }

        [XmlAttribute]
        public String Code { get; set; }
    }

    // Models for request bodies

    [Serializable]
    [XmlRootAttribute("Write")]
    public class MsgToWrite
    {
        public string Account { get; set; }
        public MsgToPost Message { get; set; }
    }

    [XmlTypeAttribute("Message")]
    public class MsgToPost
    {
        public string Content { get; set; }
    }

    [Serializable]
    public class SubscribeList
    {
        public string Account { get; set; }
        public Guid ListID { get; set; }
    }

}