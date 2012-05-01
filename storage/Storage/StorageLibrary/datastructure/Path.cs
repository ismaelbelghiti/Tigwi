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
        public const string U_PASSWORD = "password/";

        // account container
        public const string A_IDBYNAME = "idbyname/";
        public const string A_INFO = "info/";
        public const string A_USERS = "users/";
        public const string A_ADMINID = "adminid/";

        // list container
        public const string L_INFO = "info/";
        public const string L_PERSO = "personnallist/";
        public const string L_OWNER = "owner/";
        public const string L_OWNEDLISTS_PUBLIC = "ownedlists/public/";
        public const string L_OWNEDLISTS_PRIVATE = "ownedlists/private/";
        public const string L_FOLLOWEDLISTS = "followedlists/";
        public const string L_FOLLOWEDLISTS_DATA = "/data";
        public const string L_FOLLOWEDLISTS_LOCK = "/lock";
        public const string L_FOLLOWINGACCOUNTS = "followingaccounts/";

        const string L_FOLLOWEDBY = "followedby/";
        const string L_FOLLOWEDBY_PUBLIC = "/public";
        const string L_FOLLOWEDBY_ALL = "/all";

        static public string LFollowedByPublic(Guid accountId)
        {
            return L_FOLLOWEDBY + accountId + L_FOLLOWEDBY_PUBLIC;
        }
        static public string LFollowedByAll(Guid accountId)
        {
            return L_FOLLOWEDBY + accountId + L_FOLLOWEDBY_ALL;
        }

        public const string L_FOLLOWEDACCOUNTS = "followedaccounts/";
        public const string L_FOLLOWEDACC_DATA = "/data";
        public const string L_FOLLOWEDACC_LOCK = "/lock";

        // message container
        public const string M_LISTMESSAGES = "listmessages/";
        public const string M_ACCOUNTMESSAGES = "accountmessages/";
        public const string M_MESSAGE = "message/";
        public const string M_TAGGEDMESSAGES = "taggedmessages/";

    }
}
