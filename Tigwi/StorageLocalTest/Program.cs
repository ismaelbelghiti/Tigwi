using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageCommon;
using StorageLibrary;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLocalTest
{
    class Program
    {
        static void Main(string[] args)
        {

            // Test code here
            Storage storage = new Storage("ulyssestorage", "");

            Console.WriteLine("Init ok");

            try 
            {
                Guid noneid = storage.User.GetId("test");
                Console.WriteLine("Error, User.GetId return an id where it shouldn't.");
            } catch (UserNotFound) { }

            Console.Write(".");

            try
            {
                Guid noneid = storage.Account.GetId("test");
                Console.WriteLine("Error, Account.GetId return an id where it shouldn't.");
            } catch (AccountNotFound) { }

            Console.Write(".");
            Guid userid = storage.User.Create("test", "bidon@test.com");

            try
            {
                Guid sameid = storage.User.GetId("test");
                if (userid != sameid) Console.WriteLine("Error, User.GetId");
            } catch (UserNotFound) { Console.WriteLine("Error, User.GetId does not found a new Id."); }
            Console.Write(".");

            try
            {
                Guid sameuser = storage.User.Create("test", "bidon@test2.com");
                Console.WriteLine("Error, User.Create does not detect existing user");
            }
            catch (UserAlreadyExists) { }
            Console.Write(".");

            try
            {
                IUserInfo userinfo = storage.User.GetInfo(userid);
                if (userinfo.Login != "test" || userinfo.Email != "bidon@test.com") Console.WriteLine("Error, User.GetInfo");
            }
            catch (UserNotFound) { Console.WriteLine("Error, User.GetInfo does not found a new Id."); }
            Console.Write(".");

            try
            {
                storage.User.SetInfo(userid, "email@test.com");
                IUserInfo newuserinfo = storage.User.GetInfo(userid);
                if ( newuserinfo.Email != "email@test.com") Console.WriteLine("Error, User.SetInfo");
            } catch (UserNotFound) { Console.WriteLine("Error, User.GetInfo does not found an Id."); }
            Console.Write(".");

            Guid accountid = storage.Account.Create(userid, "test", "description");

            try
            {
                Guid sameaccountid = storage.Account.GetId("test");
                if (accountid != sameaccountid) Console.WriteLine("Error, Account.GetId");
            } catch (AccountNotFound) { Console.WriteLine("Error, Account.GetId does not found a new Id."); }
            Console.Write(".");

            try
            {
                Guid sameaccount = storage.Account.Create(userid, "test", "description2");
                Console.WriteLine("Error, Account.Create does not detect existing account"); 
            } catch (AccountAlreadyExists) { }
            Console.Write(".");

            try
            {
                IAccountInfo accountinfo = storage.Account.GetInfo(accountid);
                if (accountinfo.Name != "test" || accountinfo.Description != "description")
                    Console.WriteLine("Error, Account.GetInfo");
            }
            catch (UserNotFound)  { Console.WriteLine("Error, Account.GetInfo does not found a new Id."); }
            Console.Write(".");

            HashSet<Guid> accounts = storage.User.GetAccounts(userid);
            if (!accounts.Contains(accountid))
                Console.WriteLine("Error, User.GetAccounts");
            Console.Write(".");

            HashSet<Guid> users = storage.Account.GetUsers(accountid);
            if (!users.Contains(userid))
                Console.WriteLine("Error, Account.GetUsers");
            Console.Write(".");

            Guid useridbis = storage.User.Create("user2", "user@mail.com");
            storage.Account.Add(accountid, useridbis);
            users = storage.Account.GetUsers(accountid);
            if (!users.Contains(useridbis))
                Console.WriteLine("Error, Account.Add");
            Console.Write(".");

            Guid admin = storage.Account.GetAdminId(accountid);
            if (admin != userid)
                Console.WriteLine("Error, Account.GetAdmin");
            Console.Write(".");

            storage.Account.SetAdminId(accountid, useridbis);
            admin = storage.Account.GetAdminId(accountid);
            if (admin != useridbis)
                Console.WriteLine("Error, Account.SetAdmin");
            Console.Write(".");

            try
            {
                storage.Account.Remove(accountid, useridbis);
                Console.WriteLine("Error, Account.Remove doesn't detect admin");
            } catch (UserIsAdmin) { }
            Console.Write(".");

            storage.Account.Remove(accountid, userid);
            storage.Account.Delete(accountid);
            storage.User.Delete(userid);
            storage.User.Delete(useridbis);

            try
            {
                Guid nonexistent = storage.Account.GetId("nametest");
                Console.WriteLine("Error, Account.Delete does not delete.");
            } catch (AccountNotFound) { }
            Console.Write(".");

            try
            {
                Guid nonexistent = storage.User.GetId("settest");
                Console.WriteLine("Error, User.Delete does not delete.");
            } catch (UserNotFound) { }
            Console.Write(".\nEND\n");
            // End test code

            Console.ReadLine();
        }
    }
}
