using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using StorageLibrary;

namespace Tigwi_test
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            // Test code here
            Storage storage = new Storage("eyaris", "StorageConnectionString");

            UserStorage userstorage = new UserStorage(storage.connexion);
            AccountStorage accountstorage = new AccountStorage(storage.connexion);

            Guid userid = userstorage.Create("test", "bidon@test.com");
            Guid sameid = userstorage.GetId("test");
            if (userid != sameid) Trace.WriteLine("Error, User.GetId");

            IUserInfo userinfo = userstorage.GetInfo(userid);
            if (userinfo.Login != "test" || userinfo.Email != "bidon@test.com")
                Trace.WriteLine("Error, User.GetInfo");

            userstorage.SetInfo(userid, "settest", "email@test.com");
            userinfo = userstorage.GetInfo(userid);
            if (userinfo.Login != "settest" || userinfo.Email != "email@test.com")
                Trace.WriteLine("Error, User.SetInfo");

            Guid accountid = accountstorage.Create(userid, "test", "description");
            sameid = accountstorage.GetId("test");
            if (accountid != sameid) Trace.WriteLine("Error, Account.GetId");

            IAccountInfo accountinfo = accountstorage.GetInfo(accountid);
            if (accountinfo.Name != "test" || accountinfo.Description != "description")
                Trace.WriteLine("Error, Account.GetInfo");

            accountstorage.SetInfo(accountid, "nametest", "descriptiontest");
            accountinfo = accountstorage.GetInfo(accountid);
            if (accountinfo.Name != "nametest" || accountinfo.Description != "descriptiontest")
                Trace.WriteLine("Error, Account.SetInfo");

            HashSet<Guid> accounts = userstorage.GetAccounts(userid);
            if (! accounts.Contains(accountid))
                Trace.WriteLine("Error, User.GetAccounts");

            HashSet<Guid> users = accountstorage.GetUsers(accountid);
            if (! users.Contains(userid))
                Trace.WriteLine("Error, Account.GetUsers");

            Guid useridbis = userstorage.Create("user2", "user@mail.com");
            accountstorage.Add(accountid, useridbis);
            if (!users.Contains(useridbis))
                Trace.WriteLine("Error, Account.Add");

            Guid admin = accountstorage.GetAdminId(accountid);
            if (admin != userid)
                Trace.WriteLine("Error, Account.GetAdmin");

            accountstorage.SetAdminId(accountid, useridbis);
            admin = accountstorage.GetAdminId(accountid);
            if (admin != useridbis)
                Trace.WriteLine("Error, Account.SetAdmin");

            accountstorage.Remove(accountid, userid);
            accountstorage.Delete(accountid);
            userstorage.Delete(userid);
            userstorage.Delete(useridbis);

            // End test code

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
