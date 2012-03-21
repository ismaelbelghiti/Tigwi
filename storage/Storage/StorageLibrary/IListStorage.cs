using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IListStorage
    {
        /// <summary>
        /// Return the id of the new list
        /// </summary>
        /// <param name="owner_id"></param>
        /// <param name="list_name"></param>
        /// <param name="is_private"></param>
        /// <returns></returns>
        int Create_list(int owner_id, string list_name, bool is_private);
        void Delete_list(int list_id);
        void Set_list_name(int list_id, string new_name);
        void Set_list_access(int list_id, bool new_access);
        /// <summary>
        /// Return the ids of the public lists with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        HashSet<int> Get_lists_by_name(string name);
        void Add_subscriber(int list_id, int account_id);
        void Remove_subscriber(int list_id, int account_id);
        void Add_subscription(int list_id, int account_id);
        void Remove_subscription(int list_id, int account_id);
        HashSet<int> Get_account_owned_lists(int account_id, bool with_private);
        /// <summary>
        /// Return the ids of the lists to which the account is subscribed
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        HashSet<int> Get_account_subscribed_lists(int account_id);
        /// <summary>
        /// Return the ids of the lists in which the account is
        /// </summary>
        /// <param name="account_id"></param>
        /// <returns></returns>
        HashSet<int> Get_account_following_lists(int account_id);
        /// <summary>
        /// Return the ids of the accounts in the list
        /// </summary>
        /// <param name="list_id"></param>
        /// <returns></returns>
        HashSet<int> Get_list_subscriptions(int list_id);
        /// <summary>
        /// Return the ids of the accounts subscribed to the list
        /// </summary>
        /// <param name="list_id"></param>
        /// <returns></returns>
        HashSet<int> Get_list_subscribers(int list_id);
    }
}
