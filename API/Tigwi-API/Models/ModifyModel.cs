using System;
using System.Collections.Generic;
using System.Linq;
using StorageLibrary;
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

}