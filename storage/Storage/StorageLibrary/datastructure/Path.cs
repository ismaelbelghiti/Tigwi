using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    class Path
    {
        // user container
        public const string U_INFO = "info/";
        public const string U_ACCOUNTS = "accounts/";
        public const string U_IDBYLOGIN = "idbylogin/";
        public const string U_ACC_DATA = "/data";
        public const string U_ACC_LOCK = "/lock";

        // account container
        public const string A_IDBYNAME = "idbyname/";
        public const string A_INFO = "info/";
        public const string A_USERS = "users/";
        public const string A_ADMINID = "adminid/";

        // list container
        public const string L_INFO = "info/";
        public const string L_PERSO = "personnallist/";
        public const string L_OWNER = "owner/";
        public const string L_OWNEDLISTS_PUBLIC = "ownedlists/public";
        public const string L_OWNEDLISTS_PRIVATE = "ownedlists/private";
        public const string L_FOLLOWEDLISTS = "followedlists/";
        public const string L_FOLLOWEDLISTS_DATA = "/data";
        public const string L_FOLLOWEDLISTS_LOCK = "/lock";
        public const string L_FOLLOWINGACCOUNTS = "followingaccounts/";
        public const string L_FOLLOWEDBY = "followedby/";
        public const string L_FOLLOWEDACCOUNTS = "followedaccounts/";
        public const string L_FOLLOWEDACC_DATA = "/data";
        public const string L_FOLLOWEDACC_LOCK = "/lock";

        // message container
    }
}
