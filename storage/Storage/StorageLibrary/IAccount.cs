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
        List<int> Account_followedlists { get; set; }
        List<int> Account_followinglists { get; set; }
        int Account_adminid { get; set; }
        List<int> Account_messages { get; set; }
        List<int> Account_taggedmsgs { get; set; }
    }
}
