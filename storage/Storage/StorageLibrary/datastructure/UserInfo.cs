using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    [Serializable]
    class UserInfo : IUserInfo
    {
        public UserInfo(string login, string avatar ,string email)
        {
            Login = login;
            Avatar = Avatar;
            Email = email;
        }

        public string Login { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }
    }
}
