using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StorageLibrary
{
    interface IAccountInfo : ISerializable
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
