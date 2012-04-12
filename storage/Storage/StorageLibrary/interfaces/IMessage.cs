using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StorageLibrary
{
    public interface IMessage
    {
        Guid Id {get; set;}
        Guid PosterId {get; set;}
        string PosterName { get; set; }
        string PosterAvatar { get; set; }
        DateTime Date {get; set;}
        string Content { get; set; }
    }
}
