using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    class UserInfo
    {
        private string login;
        private string email;

        string Login 
        { 
            get 
            { 
                return login; 
            }
            set
            { 
                login = value; 
            } 
        }
        string Email 
        { 
            get 
            { 
                return email;
            } 
            set 
            { 
                email = value;
            } 
        }

        UserInfo(string login, string email)
        {
            Login = login;
            Email = email;
        }
    }
}
