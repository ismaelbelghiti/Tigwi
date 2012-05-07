using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Tigwi.Storage.Library
{
    [ProtoContract]
    public class UserInfo : IUserInfo
    {
        public UserInfo(string login, string avatar ,string email)
        {
            Login = login;
            Avatar = Avatar;
            Email = email;
        }

        public UserInfo()
        {
            Login = null;
            Avatar = null;
            Email = null;
        }

        [ProtoMember(1)]
        public string Login { get; set; }

        [ProtoMember(2)]
        public string Avatar { get; set; }

        [ProtoMember(3)]
        public string Email { get; set; }
    }
}
