using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    class AccountInfo
    {
        private string name;
        private string admin;
        private string description;

        string Name
        { 
            get 
            { 
                return name; 
            }
            set
            { 
                name = value; 
            } 
        }
        string Admin 
        { 
            get 
            { 
                return admin;
            } 
            set 
            { 
                admin = value;
            } 
        }
        string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        AccountInfo(string name, string admin, string description)
        {
            Name = name;
            Admin = admin;
            Description = description;
        }
    }
}
