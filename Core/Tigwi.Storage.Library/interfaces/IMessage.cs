#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tigwi.Storage.Library
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
