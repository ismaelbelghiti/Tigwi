using System;
using System.Collections.Generic;
using System.Linq;
using StorageLibrary;
using System.Xml.Serialization;

namespace Tigwi_API.Models
{

    // models for answers to GET requests
    [Serializable]
    public abstract class Content
    {}

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

    [Serializable]
    public class Messages : Content
    {
        public Messages()
        {
            Message = new List<Message>();
            Size = 0;
        }

        public Messages(List<Message> msgs)
        {
            Message = msgs;
            Size = msgs.Count();
        }

        public Messages(List<IMessage> msgs, IStorage storage)
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
    public class Account : Content
    {
        public Account(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Accounts : Content
    {
        public Accounts()
        {
            Account = new List<Account>();
            Size = 0;
        }

        public Accounts(List<Account> accounts)
        {
            Account = accounts;
            Size = accounts.Count();
        }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlElement] public List<Account> Account;
    }

    // models to answer to requests with errors (can be empty) messages

    [Serializable]
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

    //General class to send
    [Serializable]
    public class Answer
    {
        public Answer()
        {
            Error = null;
            Content = null;
        }

        public Answer(Error error)
        {
            Error = error;
            Content = null;
        }

        public Answer(Content content)
        {
            Error = null;
            Content = content;
        }   
        public Error Error;
        public Content Content;
    }
}