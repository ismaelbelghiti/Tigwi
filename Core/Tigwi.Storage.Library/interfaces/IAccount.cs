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
using System.Xml.Serialization;

namespace Tigwi.Storage.Library
{
    public interface IAccountInfo
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
