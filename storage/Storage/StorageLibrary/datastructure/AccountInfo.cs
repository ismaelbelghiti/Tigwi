using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary.datastructure
{
    [Serializable]
    class AccountInfo : IAccountInfo
    {
        public AccountInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
