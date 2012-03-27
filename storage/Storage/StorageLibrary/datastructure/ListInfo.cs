using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary.datastructure
{
    [Serializable]
    class ListInfo : IListInfo
    {
        public ListInfo(string name, string description, bool isPrivate)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate; 
        }      

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }
    }
}
