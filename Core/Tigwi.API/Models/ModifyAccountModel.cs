using System;
using System.Xml.Serialization;

namespace Tigwi.API.Models
{
    // Models for request bodies

    public class BaseRequest
    {
        public string AccountName { get; set; }
        public Guid? AccountId { get; set; }
        public string Key { get; set; }
    }


    [Serializable]
    [XmlRootAttribute("Write")]
    public class MsgToWrite:BaseRequest
    {
        public MsgToPost Message { get; set; }
    }

    [Serializable]
    [XmlTypeAttribute("Message")]
    public class MsgToPost
    {
        public string Content { get; set; }
    }

    [Serializable]
    public class ActionOnMessage:BaseRequest
    {
        public Guid? MessageId { get; set; }
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
    public class SubscribeList:BaseRequest
    {
        public Guid? Subscription { get; set; }
    }

    [Serializable]
    public class ListInfo
    {
        public string Name;
        public string Description;
        public bool IsPrivate;
    }

    [Serializable]
    public class CreateList:BaseRequest
    {

        public ListInfo ListInfo { get; set; }
    }
}