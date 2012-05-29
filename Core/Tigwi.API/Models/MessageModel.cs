#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Xml.Serialization;

namespace Tigwi.API.Models
{

    // Add a message

    [Serializable]
    [XmlRootAttribute("Write")]
    public class MsgToWrite:BaseRequest
    {
        public string Message { get; set; }
    }

    // Remove a message

    [Serializable]
    [XmlRootAttribute("Delete")]
    public class MsgToDelete
    {
        public Guid? MessageId { get; set; }
    }

    // Other actions on messages

    [Serializable]
    public class ActionOnMessage:BaseRequest
    {
        public Guid? MessageId { get; set; }
    }

    [Serializable]
    [XmlRootAttribute("Copy")]
    public class CopyMsg : ActionOnMessage{}

    [Serializable]
    public class Tag : ActionOnMessage{}

    [Serializable]
    public class Untag : ActionOnMessage{}

    
}