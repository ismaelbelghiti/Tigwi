using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace StorageLibrary
{
    [ProtoContract]
    public class ListInfo : IListInfo
    {
        public ListInfo(string name, string description, bool isPrivate, bool isPersonnal)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;
            IsPersonnal = isPersonnal;
        }

        public ListInfo()
        {
            Name = null;
            Description = null;
            IsPrivate = false;
            IsPersonnal = false;
        }

        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string Description { get; set; }

        [ProtoMember(3)]
        public bool IsPrivate { get; set; }

        [ProtoMember(4)]
        public bool IsPersonnal { get; set; }
    }
}
