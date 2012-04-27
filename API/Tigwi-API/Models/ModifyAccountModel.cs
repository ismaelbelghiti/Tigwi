using System;
using System.Xml.Serialization;

namespace Tigwi_API.Models
{
    // Models for request bodies

    [Serializable]
    [XmlRootAttribute("Write")]
    public class MsgToWrite
    {
        //TODO: find how to make some fields optional
        public string AccountName { get; set; }
        public Guid AccountId { get; set; }
        public MsgToPost Message { get; set; }
    }

    [Serializable]
    [XmlTypeAttribute("Message")]
    public class MsgToPost
    {
        public string Content { get; set; }
    }

    [Serializable]
    public class ActionOnMessage
    {
        //TODO: find how to make some fields optional
        public string AccountName { get; set; }
        public Guid AccountId { get; set; }
        public Guid MessageId { get; set; }
    }

    [Serializable]
    [XmlRootAttribute("Delete")]
    public class MsgToDelete : ActionOnMessage{}

    [Serializable]
    [XmlRootAttribute("Copy")]
    public class CopyMsg : ActionOnMessage{}

    [Serializable]
    public class Tag : ActionOnMessage{}

    [Serializable]
    public class Untag : ActionOnMessage { }

    [Serializable]
    public class SubscribeList
    {
        public string Account { get; set; }
        public Guid Subscription { get; set; }
    }

    [Serializable]
    public class ListInfo
    {
        public string Name;
        public string Description;
        public bool IsPrivate;
    }

    [Serializable]
    public class CreateList
    {
        public string Account { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    [Serializable]
    public class ChangeDescription
    {
        //TODO: find how make optional some fields fot the request
        public string AccountName { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class AccountUser
    {
        //TODO: find how make optional some fields fot the request
        public string AccountName { get; set; }
        public Guid AccountId { get; set; }
        public string UserLogin { get; set; }
        public Guid UserId { get; set; }
    }

    [Serializable]
    public class AddUser : AccountUser{}

    [Serializable]
    public class RemoveUser : AccountUser{}

    [Serializable]
    public class ChangeAdministrator : AccountUser { }

}