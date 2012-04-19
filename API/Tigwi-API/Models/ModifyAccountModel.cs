using System;
using System.Xml.Serialization;

namespace Tigwi_API.Models
{
    // Models for request bodies

    [Serializable]
    [XmlRootAttribute("Write")]
    public class MsgToWrite
    {
        public string Account { get; set; }
        public MsgToPost Message { get; set; }
    }

    [Serializable]
    [XmlTypeAttribute("Message")]
    public class MsgToPost
    {
        public string Content { get; set; }
    }

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

}