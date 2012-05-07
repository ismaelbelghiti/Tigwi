using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Tigwi.Storage.Library
{
    public interface IUserInfo
    {
        string Login { get; set; }

        string Avatar { get; set; }

        string Email { get; set; }
    }
}
