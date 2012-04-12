using System;
using System.Collections.Generic;
using System.Linq;
using StorageLibrary;
using System.Xml.Serialization;

namespace Tigwi_API.Models
{

    // models for answers to GET requests

    [Serializable]
    public class Message
    {
        public Message(IMessage msg, IStorage storage)
        {
            Id = msg.Id;
            PostTime = msg.Date;
            Poster = storage.Account.GetInfo(msg.PosterId).Name;
            Content = msg.Content;
        }

        public Guid Id { get; set; }

        public DateTime PostTime { get; set; }

        public string Poster { get; set; }

        public string Content { get; set; }
    }

    public class MessageList
    {
        public MessageList()
        {
            Message = new List<Message>();
            Size = 0;
        }

        public MessageList(List<Message> msgs)
        {
            Message = msgs;
            Size = msgs.Count();
        }

        public MessageList(List<IMessage> msgs, IStorage storage)
        {
            Message = msgs.ConvertAll(ancient => new Message(ancient, storage));
            Size = msgs.Count();
        }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlElement]
        public List<Message> Message;
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

    // models to answer to requests with errors (can be empty) messages

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
}