using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IAccount
    {
        int Account_id { get; set; }
        string Account_name { get; set; }
        string Account_description { get; set; }
        HashSet<int> Account_followedlists { get; set; }
        HashSet<int> Account_followinglists { get; set; }
        int Account_adminid { get; set; }
        HashSet<int> Account_messages { get; set; }
        HashSet<int> Account_taggedmsgs { get; set; }
    }
}
