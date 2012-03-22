using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IList
    {
        int List_id { get; set; }
        int List_ownerid { get; set; }
        string List_name { get; set; }
        bool List_isprivate { get; set; }
    }
}
