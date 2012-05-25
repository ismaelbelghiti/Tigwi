using System;
using System.Collections.Generic;
using System.Linq;
using Tigwi.Storage.Library;
using System.Xml.Serialization;

namespace Tigwi.API.Models
{
    // Models for request bodies

    public class BaseRequest
    {
        public string AccountName { get; set; }
        public Guid? AccountId { get; set; }
    }

    // Models to answer to requests
    
    [Serializable]
    public abstract class Content
    {}

    [Serializable]
    public class Message 
    {
        public Message() {}
        public Message(IMessage msg, IStorage storage)
        {
            Id = msg.Id.ToString();
            PostTime = msg.Date;
            Poster = storage.Account.GetInfo(msg.PosterId).Name;
            Content = msg.Content;
        }

        public string Id { get; set; }

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
        public Account () {}
        public Account(Guid id, string name, string description)
        {
            Id = id.ToString();
            Name = name;
            Description = description;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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

    [Serializable]
    public class User : Content
    {
        public User () {}
        public User (IUserInfo user, Guid userId)
        {
            Login = user.Login;
            Avatar = user.Avatar;
            Email = user.Email;
            Id = userId.ToString();
        }

        public string Login { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
    }

    [Serializable]
    [XmlType("List")]
    public class ListApi
    {
        public ListApi() { }
        public ListApi(Guid id, string name)
        {
            Id = id.ToString();
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Lists : Content
    {
        public Lists()
        {
            List = new List<ListApi>();
            Size = 0;
        }
        public Lists(List<ListApi> list)
        {
            List = list;
            Size = list.Count();
        }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlElement]
        public List<ListApi> List;
    }

    [Serializable]
    public class NewObject : Content
    {
        public NewObject() {}
        public NewObject(Guid id)
        {
            Id = id.ToString();
        }

        [XmlAttribute]
        public string Id;
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

    // General class to send information
    [Serializable]
    [XmlInclude(typeof(Messages))]
    [XmlInclude(typeof(Account))]
    [XmlInclude(typeof(Accounts))]
    [XmlInclude(typeof(Lists))]
    [XmlInclude(typeof(User))]
    [XmlInclude(typeof(NewObject))]
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

    // A class with to generate the key for API authentication
    [Serializable]
    public class identityForApiKey
    {
        public string accountName;
        public string password;
        public string applicationName;
    }
}