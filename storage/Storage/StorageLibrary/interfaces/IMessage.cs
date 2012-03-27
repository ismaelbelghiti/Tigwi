using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StorageLibrary
{
    public interface IMessage
    {
        int Id {get; set;}
        int PosterId {get; set;}
        DateTime Date {get; set;}
        string Content { get; set; }
    }
}
