using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    [Serializable]
    class ListInfo : IListInfo
    {
        public ListInfo(string name, string description, bool isPrivate, bool isPersonnal)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;
            IsPersonnal = isPersonnal;
        }      

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsPersonnal { get; set; }
    }
}
