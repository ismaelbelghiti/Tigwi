using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Tigwi.Storage.Library
{
    [ProtoContract]
    public class AccountInfo : IAccountInfo
    {
        public AccountInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public AccountInfo()
        {
            Name = null;
            Description = null;
        }

        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string Description { get; set; }
    }
}
