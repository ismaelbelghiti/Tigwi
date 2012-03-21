using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IMsgStorage
    {
        string get_accounts_msg_from(List<int> accounts, int first_msg, int msg_number);
        string get_lists_msg_from(List<int> lists, int first_msg, int msg_number);
        string get_accounts_msg_to(List<int> accounts, int last_msg, int msg_number);
        string get_lists_msg_to(List<int> lists, int last_msg, int msg_number);
    }
}
