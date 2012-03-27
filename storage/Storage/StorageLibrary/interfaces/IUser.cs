using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StorageLibrary
{
    interface IUserInfo : ISerializable
    {
        string Login { get; set; }
        string Email { get; set; }
    }
}
