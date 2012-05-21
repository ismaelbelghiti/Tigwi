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
        public UserInfo(string login, string avatar ,string email, Guid mainAccountId)
        {
            Login = login;
            Avatar = Avatar;
            Email = email;
            MainAccountId = mainAccountId;
        }

        public UserInfo()
        {
            Login = null;
            Avatar = null;
            Email = null;
            MainAccountId = new Guid();
        }

        [ProtoMember(1)]
        public string Login { get; set; }

        [ProtoMember(2)]
        public string Avatar { get; set; }

        [ProtoMember(3)]
        public string Email { get; set; }

        [ProtoMember(4)]
        public Guid MainAccountId { get; set; }
    }
}
