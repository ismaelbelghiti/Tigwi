using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    interface IUser
    {
        int User_id { get; set; }
        string User_login { get; set; }
        string User_email { get; set; }
    }
}
