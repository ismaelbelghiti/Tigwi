using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IMsgStorage
    {
        string Get_msg(int msg_id);
        string Get_poster(int msg_id);
        DateTime Get_date(int msg_id);
        HashSet<int> Get_accounts_msg_from(HashSet<int> accounts, int first_msg, int msg_number);
        HashSet<int> Get_lists_msg_from(HashSet<int> lists, int first_msg, int msg_number);
        HashSet<int> Get_accounts_msg_to(HashSet<int> accounts, int last_msg, int msg_number);
        HashSet<int> Get_lists_msg_to(HashSet<int> lists, int last_msg, int msg_number);
        void Tag_msg(int account_id, int msg_id);
        void Untag_msg(int accound_id, int msg_id);
        HashSet<int> Get_tagged_msg_from(int accound_id, int first_msg, int msg_number);
        HashSet<int> Get_tagged_msg_to(int account_id, int last_msg, int msg_number);
        /// <summary>
        /// Return the id of the new message
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        int Post_msg(int account_id, string msg);
        /// <summary>
        /// Return the id of the new message
        /// </summary>
        /// <param name="accound_id"></param>
        /// <param name="msg_id"></param>
        /// <returns></returns>
        int Copy_msg(int accound_id, int msg_id);
        void Remove_msg(int msg_id);
    }
}
