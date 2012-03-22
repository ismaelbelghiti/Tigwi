using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IAccountStorage
    {
        AccountInfo Get_account_info(int account_id);
        int Get_account_id(string name);
        void Set_account_info(int account_i, AccountInfo info);
        void Add_account(int user_id, int account_id);
        void Remove_account(int user_id, int account_id);
        /// <summary>
        /// Return the id of the new account
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int Create_account(AccountInfo info);
        void Delete_account(int account_id);
    }
}
