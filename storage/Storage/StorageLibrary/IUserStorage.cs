using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IUserStorage
    {
        UserInfo Get_user_info(int user_id);
        int Get_user_id(string login);
        void Set_user_info(int user_id, UserInfo new_info);
        /// <summary>
        /// Return the id of the new user
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        int Create_user(UserInfo info);
        void Delete_user(int user_id);
    }
}
