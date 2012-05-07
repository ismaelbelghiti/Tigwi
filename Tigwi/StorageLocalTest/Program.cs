using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageLibrary;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLocalTest
{
    class Program
    {
        const string azureAccountName = "ulyssestorage";
        const string azureAccountKey = "fc2HTyfP0m2r3zlNYmMc3Pjvbfmy63ovoCP9Zkz0yoyuId3AeyrTswLcye2VDr3hzDvAQbdeKUlXBX3lFTcNWQ==";

        static void TestUser(IStorage storage)
        {
            Console.WriteLine("TestUser");

            #region Guid GetId(string login)
                // "UserNotFound" if no user has this login

                // test exception "UserNotFound"
                try
                {
                    Guid noneid = storage.User.GetId("userThatDontExists");
                    Console.WriteLine("Error, User.GetId return an id where it shouldn't.");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    Guid id = storage.User.GetId("userThatExists");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetId does not found an Id."); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId"); }
                Console.Write(".");
            #endregion

            #region IUserInfo GetInfo(Guid userId)
                //"UserNotFound">if no user has this ID

                // test exception "UserNotFound"

                try
                {
                    IUserInfo userInfo = storage.User.GetInfo(new Guid());
                    Console.WriteLine("Error, User.GetInfo return an info where it shouldn't.");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetInfo"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    IUserInfo userinfo = storage.User.GetInfo(storage.User.GetId("userThatExists"));
                    if (userinfo.Login != "userThatExists" || userinfo.Email != "userThatExists@gmail.com")
                        Console.WriteLine("Error, User.GetInfo");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetInfo does not found an Id."); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetInfo"); }
                Console.Write(".");
            #endregion
            
            #region void SetInfo(Guid userId, string email)
                //"UserNotFound">if no user has this ID

                // test exception "UserNotFound"

                try
                {
                    storage.User.SetInfo(Guid.NewGuid(), "babar@celeste.com");
                    Console.WriteLine("Error, User.SetInfo does not raise UserNotFound.");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.SetInfo"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    Guid userid = storage.User.GetId("userThatExists");
                    storage.User.SetInfo(userid, "userThatExists@notgmail.com");
                    IUserInfo newuserinfo = storage.User.GetInfo(userid);
                    if (newuserinfo.Email != "userThatExists@notgmail.com")
                        Console.WriteLine("Error, User.SetInfo");
                    storage.User.SetInfo(userid, "userThatExists@gmail.com");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.Set/GetInfo does not found an Id."); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/SetInfo/GetInfo"); }
                Console.Write(".");
            #endregion

            #region HashSet<Guid> GetAccounts(Guid userId)
                //"UserNotFound">if no user has this ID

                // test exception "UserNotFound"
                try
                {
                    Guid userid = storage.User.GetId("userThatExists");
                    Guid accountid = storage.Account.GetId("accountThatExists");
                    HashSet<Guid> accounts = storage.User.GetAccounts(userid);
                    if (!accounts.Contains(accountid))
                        Console.WriteLine("Error, User.GetAccounts");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/GetAccounts"); }
                Console.Write(".");

                //test normal behaviour
                try
                {
                    Guid userid = storage.User.GetId("userThatExists");
                    Guid accountid = storage.Account.GetId("accountThatExists");
                    HashSet<Guid> accounts = storage.User.GetAccounts(userid);
                    if (!accounts.Contains(accountid))
                        Console.WriteLine("Error, User.GetAccounts");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetAccounts"); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/GetAccounts"); }
                Console.Write(".");
            #endregion

            #region Guid Create(string login, string email, string password)
                //"UserAlreadyExists">if the login is already used

                //test exception "UserAlreadyExists"

                try
                {
                    Guid sameuser = storage.User.Create("userThatExists", "bidon@test2.com", new Byte[1]);
                    Console.WriteLine("Error, User.Create does not detect existing user");
                }
                catch (UserAlreadyExists) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.Create"); }
                Console.Write(".");

                //test normal behaviour : Done with "userThatExists"


                //void Delete(Guid userId); -> UserIsAdmin

                // test exception "UserIsAdmin"

                try
                {
                    storage.User.Delete(storage.User.GetId("userThatExists"));
                    Console.WriteLine("Error, User.Delete does not detect an admin user");
                }
                catch (UserIsAdmin) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetId/Delete"); }

                //test normal behaviour

                try
                {
                    Guid userTempId = storage.User.Create("userTemp", "bidon@test2.com", new Byte[1]);
                    storage.User.Delete(userTempId);
                    storage.User.GetId("userTemp");
                    Console.WriteLine("Error, User.Delete does not delete");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.Create/Delete/GetId"); }
                Console.Write(".");
            #endregion

            #region string GetPassword(Guid userID)
                //UserNotFound

                // test exception "UserNotFound"

                try
                {
                    storage.User.GetPassword(new Guid());
                    Console.WriteLine("Error, User.GetPassword does not detect existing user");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetPassword"); }
                Console.Write(".");

                //test normal behaviour

                try
                {
                    Byte[] pass = storage.User.GetPassword(storage.User.GetId("userThatExists"));
                    if (pass != new Byte[1])
                        Console.WriteLine("Error, User.GetPassword does not get the good password");
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.GetPassword unable to find the id"); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.GetPassword"); }
                Console.Write(".");
            #endregion

            #region void SetPassword(Guid userID, string password)
                //"UserNotFound"

                // test exception "UserNotFound"

                try
                {
                    storage.User.SetPassword(Guid.NewGuid(), new Byte[1]);
                    Console.WriteLine("Error, SetPassword doed not raise UserNotFound");
                }
                catch (UserNotFound) { }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.SetPassword"); }

                //test normal behaviour with valid pass

                try
                {
                    storage.User.SetPassword(storage.User.GetId("userThatExists"), new Byte[1]);
                    Byte[] pass = storage.User.GetPassword(storage.User.GetId("userThatExists"));
                    if (pass != new Byte[1])
                        Console.WriteLine("Error, User.SetPassword does not set the good password");
                    storage.User.SetPassword(storage.User.GetId("userThatExists"), new Byte[1]);
                }
                catch (UserNotFound) { Console.WriteLine("Error, User.SetPassword unable to find the id"); }
                catch (Exception) { Console.WriteLine("Unhandled exception in storage.User.SetPassword/GetPassword"); }
                Console.Write(".");
            #endregion

            Console.WriteLine();

        }

        static void TestAccounts(IStorage storage)
        {
            Console.WriteLine("TestAccounts");

            #region Guid GetId(string name)
                // AccountNotFound if no account has this name

                // test exception "AccountNotFound"
                try
                {
                    Guid noneid = storage.Account.GetId("accountThatDoesntExist");
                    Console.WriteLine("Error, Account.GetId return an id where it shouldn't.");
                }
                catch (AccountNotFound) { }

                //test normal behaviour
                try
                {
                    Guid id = storage.Account.GetId("accountThatExists");
                }
                catch (AccountNotFound) { Console.WriteLine("Error, Account.GetId does not find id."); }
                Console.Write(".");
            #endregion

            #region IAccountInfo GetInfo(Guid accountId)
                // "AccountNotFound" if no account has this ID

                // test exception "AccountNotFound"
                try
                {
                    IAccountInfo accountInfo = storage.Account.GetInfo(Guid.NewGuid());
                    Console.WriteLine("Error, Account.GetInfo doesnot raise AccountNotFound.");
                }
                catch (AccountNotFound) { }
                Console.Write(".");

                //test normal 
                try
                {
                    Guid accountid = storage.Account.GetId("accountThatExists");
                    IAccountInfo accountinfo = storage.Account.GetInfo(accountid);
                    if (accountinfo.Name != "accountThatExists" || accountinfo.Description != "accountThatExistsDesc")
                        Console.WriteLine("Error, Account.GetInfo");
                }
                catch (UserNotFound) { Console.WriteLine("Error, Account.GetInfo does not found a new Id."); }
                Console.Write(".");
            #endregion

            #region void SetInfo(Guid accountId, string description)
                //"AccountNotFound" if no account has ID=accountID>

                // test exception "AccountNotFound"
                try
                {
                    storage.Account.SetInfo(new Guid(), "");
                    Console.WriteLine("Error, Account.SetInfo does not raise AccountNotFound");
                }
                catch (AccountNotFound) { }

                //test normal behaviour
                try
                {
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    storage.Account.SetInfo(accountId, "accountThatExistsBadDesc");
                    IAccountInfo newaccountinfo = storage.Account.GetInfo(accountId);
                    if (newaccountinfo.Description != "accountThatExistsBadDesc")
                        Console.WriteLine("Error, Account.SetInfo");
                    storage.Account.SetInfo(accountId, "accountThatExistsDesc");
                }
                catch (AccountNotFound) { Console.WriteLine("Error, Account.SetInfo does not find an id"); }
            #endregion

            #region HashSet<Guid> GetUsers(Guid accountId)
                // AccountNotFound

                // test exception "AccountNotFound"
                try
                {
                    HashSet<Guid> users = storage.Account.GetUsers(Guid.NewGuid());
                    Console.WriteLine("Error, Account.GetUsers does not raise AccountNotFound");
                }
                catch (AccountNotFound) { }
                Console.Write(".");

                //test normal behaviour
                try
                {
                    Guid userId = storage.User.GetId("userThatExists");
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    HashSet<Guid> users = storage.Account.GetUsers(accountId);
                    if (!users.Contains(userId))
                        Console.WriteLine("Error, Account.GetUsers");
                }
                catch (AccountNotFound) { Console.WriteLine("Error, Account.GetUsers does not find an id"); }
                Console.Write(".");
            #endregion

            #region Guid GetAdminId(Guid accountId)
                // AccountNotFound

                // test exception "AccountNotFound"

                try
                {
                    storage.Account.GetAdminId(new Guid());
                    Console.WriteLine("Account.GetAdminId does not raise AccountNotFound");
                }
                catch (AccountNotFound) { }

                //test normal behaviour

                try
                {
                    Guid userId = storage.User.GetId("userThatExists");
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    Guid admin = storage.Account.GetAdminId(accountId);
                    if (admin != userId)
                        Console.WriteLine("Error, Account.GetAdminId");
                    Console.Write(".");
                }
                catch (AccountNotFound) { Console.WriteLine("Error, Account.GetAdminId"); }
            #endregion

            #region void SetAdminId(Guid accountId, Guid userId)
                // AccountNotFound if no account has this ID
                // UserNotFound if no user has this ID

                // test exception "AccountNotFound"
                try
                {
                    Guid userId = storage.User.GetId("userThatExists");
                    Guid accountId = storage.Account.GetId("accountThatDoesntExist");
                    storage.Account.SetAdminId(accountId, userId);
                    Console.WriteLine("Error, Account.SetAdminId does raise AccountNotFound");
                }
                catch (AccountNotFound) { }

                // test exception "UserNotFound"
                try
                {
                    Guid userId = storage.User.GetId("userThatDoesntExist");
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    storage.Account.SetAdminId(accountId, userId);
                    Console.WriteLine("Error, Account.SetAdminId does raise AccountNotFound");
                }
                catch (UserNotFound) { }

                //test normal behaviour
                try
                {
                    Guid userId = storage.User.GetId("userThatExists");
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    storage.Account.SetAdminId(accountId, userId);

                }
                catch { Console.WriteLine("Error, Account.SetAdminId does not find account id or user id"); }
            #endregion

            #region void Add(Guid accountId, Guid userId)
                // AccountNotFound if no account has this id
                // UserNotFound if no user has this id

                // test exception "AccountNotFound"
                try
                {
                    Guid otherUserId = storage.User.GetId("otherUserThatExists");
                    storage.Account.Add(otherUserId, otherUserId);
                    Console.WriteLine("Error, Account.Add does not raise AccountNotFound");
                }
                catch (AccountNotFound) { }
                Console.Write(".");

                // test exception "UserNotFound"
                try
                {
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    Guid otherUserId = storage.User.GetId("otherUserThatDoesnotExists");
                    storage.Account.Add(accountId, otherUserId);
                    Console.WriteLine("Error, Account.Add does not raise UsertNotFound");
                }
                catch (UserNotFound) { }
                Console.Write(".");

                //test normal behaviour
                try
                {
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    Guid otherUserId = storage.User.GetId("otherUserThatExists");
                    storage.Account.Add(accountId, otherUserId);
                    HashSet<Guid> users = storage.Account.GetUsers(accountId);
                    if (!users.Contains(otherUserId))
                        Console.WriteLine("Error, Account.Add");
                    HashSet<Guid> accounts = storage.User.GetAccounts(otherUserId);
                    if (!accounts.Contains(accountId))
                        Console.WriteLine("Error, Account.Add");

                }
                catch { Console.WriteLine("Error, Account.Add raised an exception where it should not"); }
                Console.Write(".");
            #endregion

            #region void Remove(Guid accountId, Guid userId)
                // UserIsAdmin if you try to remove the administrator from the user groups

                // test exception "UserIsAdmin"
                try
                {
                    Guid userId = storage.User.GetId("userThatExists");
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    storage.Account.Remove(accountId, userId);
                    Console.WriteLine("Error, Account.Remove doesn't detect admin");
                }
                catch (UserIsAdmin) { }
                Console.Write(".");

                //test with wrong accountId
                try
                {
                    Guid userId = storage.User.GetId("userThatExists");
                    storage.Account.Remove(Guid.NewGuid(), userId);
                }
                catch (AccountNotFound) { Console.WriteLine("Error, Account.Remove raised AccountNotFound where it should not"); }
                Console.Write(".");

                //test with wrong userId
                try
                {
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    storage.Account.Remove(accountId, Guid.NewGuid());
                }
                catch (UserNotFound) { Console.WriteLine("Error, Account.Remove raised UserNotFound where it should not"); }
                Console.Write(".");

                //test normal behaviour
                try
                {
                    Guid accountId = storage.Account.GetId("accountThatExists");
                    Guid otherUserId = storage.User.GetId("otherUserThatExists");
                    storage.Account.Add(accountId, otherUserId);
                    storage.Account.Remove(accountId, otherUserId);
                    HashSet<Guid> users = storage.Account.GetUsers(accountId);
                    if (users.Contains(otherUserId))
                        Console.WriteLine("Error, Account.Add");
                    HashSet<Guid> accounts = storage.User.GetAccounts(otherUserId);
                    if (accounts.Contains(accountId))
                        Console.WriteLine("Error, Account.Add");
                }
                catch { Console.WriteLine("Error, Account.Remove "); }
            #endregion

            #region Guid Create(Guid adminId, string name, string description)
                // UserNotFound if no account has this ID
                // AccountAlreadyExists if the name is already used

                // test exception "UserNotFound"
                try
                {
                    storage.Account.Create(Guid.NewGuid(), "acountThatExists", "description2");
                    Console.WriteLine("Error, Account.Create does not raise UserNotFound");
                }
                catch (UserNotFound) { }
                Console.Write(".");

                // test exception "AccountAlreadyExists"
                try
                {
                    Guid userId = storage.User.GetId("userThatExists");
                    storage.Account.Create(userId, "acountThatExists", "description2");
                    Console.WriteLine("Error, Account.Create does not detect existing account");
                }
                catch (AccountAlreadyExists) { }
                Console.Write(".");

                // test normal behavior
                // Done while doing init
            #endregion 

            #region void Delete(Guid accountId);
                try
                {
                    //test with wrong accountId
                    try
                    {
                        storage.Account.Delete(new Guid());
                    }
                    catch { Console.WriteLine("Error, Account.Delete raise exception where it should not"); }
                    Console.Write(".");

                    //test normal behaviour
                    Guid otherAccountId = storage.Account.GetId("otherAccountThatExists");
                    storage.Account.Delete(otherAccountId);
                    try
                    {
                        storage.Account.GetId("otherAccountThatExists");
                        Console.WriteLine("Error, Account.Delete does not delete");
                    }
                    catch { }
                    storage.Account.Create(storage.User.GetId("userThatExists"), "otherAccountThatExists", "otherAccountThatExistsDesc");
                }
                catch { Console.WriteLine("Unhandled esception in Account.Delete"); }
                Console.Write(".");
            #endregion

            Console.WriteLine();
        }

        static void TestList(IStorage storage, Guid listIdThatExists)
        {
            Console.WriteLine("TestList");

            #region IListInfo GetInfo(Guid listId)

            // test exception "ListNotFound"
            try
            {
                storage.List.GetInfo(new Guid());
                Console.WriteLine("Error, List.GetInfo does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetInfo"); }
            Console.Write(".");

            //test normal behaviour

            try
            {
                IListInfo listInfo = storage.List.GetInfo(listIdThatExists);
                if (listInfo.Name != "listThatExists")
                    Console.WriteLine("List.GetInfo returns wrong info");
            }
            catch (UserNotFound) { Console.WriteLine("Error, List.GetInfo does not found an id"); }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetInfo"); }
            Console.Write(".");

            #endregion

            #region void SetInfo(Guid listId, string name, string description, bool isPrivate);

            //test exception ListNotFound
            try
            {
                storage.List.SetInfo(new Guid(), "babar", "babar is cool", false);
                Console.WriteLine("Error, List.SetInfo does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.SetInfo"); }
            Console.Write(".");

            //test exception IsPersonnalList

            try
            {
                Guid idPersList = storage.List.GetPersonalList(storage.Account.GetId("accountThatExists"));
                storage.List.SetInfo(idPersList, "babar", "babar", false);
                Console.WriteLine("Error, List.SetInfo does not raise IsPersonnalList");
            }
            catch (IsPersonnalList) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.SetInfo"); }
            Console.Write(".");

            //test normal behaviour

            try
            {
                storage.List.SetInfo(listIdThatExists, "listThatExists", "Babar", false);
                if (storage.List.GetInfo(listIdThatExists).Description != "Babar")
                    Console.WriteLine("Error, List.SetInfo does not change info");
                storage.List.SetInfo(listIdThatExists, "listThatExists", "Yeah", false);
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.SetInfo"); }
            Console.Write(".");

            #endregion

            #region Guid GetOwner(Guid listId)

            //test exception ListNotFound

            try
            {
                storage.List.GetOwner(new Guid());
                Console.WriteLine("Error, List.GetOwner does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetOwner"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                Guid ownerId = storage.List.GetOwner(listIdThatExists);
                if (ownerId != storage.Account.GetId("accountThatExists"))
                    Console.WriteLine("Error, List.GetOwner returned bad owner");
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetOwner"); }
            Console.Write(".");

            #endregion

            #region Guid GetPersonalList(Guid accountId)

            //test exception AccountNotFound

            try
            {
                storage.List.GetPersonalList(new Guid());
                Console.WriteLine("Error, List.GetPersonalList does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetPersonalList"); }
            Console.Write(".");

            //test normal beahaviour : already tested

            #endregion

            #region Guid Create(Guid ownerId, string name, string description, bool isPrivate)

            //test exception AccountNotFound

            try
            {
                storage.List.Create(new Guid(), "babar", "babar", false);
                Console.WriteLine("Error, List.Create does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Create"); }
            Console.Write(".");

            //test normal beahaviour : already tested

            #endregion

            #region void Delete(Guid id)

            //test exception IsPersonnalList

            try
            {
                storage.List.Delete(storage.List.GetPersonalList(storage.Account.GetId("accountThatExists")));
                Console.WriteLine("Error, List.Delete does not raise IsPersonnalList");
            }
            catch (IsPersonnalList) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Delete"); }
            Console.Write(".");

            //test with wrong id

            try
            {
                storage.List.Delete(new Guid());
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Delete with wrong id"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                Guid listTempId = storage.List.Create(storage.Account.GetId("accountThatExists"), "babar", "babar", false);
                storage.List.Delete(listTempId);

                try
                {
                    storage.List.GetInfo(listTempId);
                    Console.WriteLine("Error, List.Delete does not delete");
                }
                catch (ListNotFound) { }
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Delete"); }
            Console.Write(".");

            #endregion

            #region void Follow(Guid listId, Guid accountId)

            //test exception AccountNotFound

            try
            {
                storage.List.Follow(listIdThatExists, new Guid());
                Console.WriteLine("Error, List.Follow does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Follow"); }
            Console.Write(".");

            //test exception ListNotFound

            try
            {
                storage.List.Follow(new Guid(), storage.Account.GetId("accountThatExists"));
                Console.WriteLine("Error, List.Follow does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Follow"); }
            Console.Write(".");

            //test normal beahaviour : already tested

            #endregion

            #region void Unfollow(Guid listId, Guid accountId)

            //test exception AccountNotFound

            try
            {
                storage.List.Unfollow(listIdThatExists, new Guid());
                Console.WriteLine("Error, List.Unfollow does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Unfollow"); }
            Console.Write(".");

            //test exception ListNotFound

            try
            {
                storage.List.Unfollow(new Guid(), storage.Account.GetId("accountThatExists"));
                Console.WriteLine("Error, List.Unfollow does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Unfollow"); }
            Console.Write(".");

            //test exception AccountIsOwner

            try
            {
                storage.List.Unfollow(listIdThatExists, storage.Account.GetId("accountThatExists"));
                Console.WriteLine("Error, List.Unfollow does not raise AccountIsOwner");
            }
            catch (AccountIsOwner) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Unfollow"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                Guid otherAccountId = storage.Account.GetId("otherAccountThatExists");
                storage.List.Unfollow(listIdThatExists, otherAccountId);
                HashSet<Guid> listsFollowed = storage.List.GetAccountFollowedLists(otherAccountId, false);
                HashSet<Guid> followers = storage.List.GetFollowingAccounts(listIdThatExists);

                if (listsFollowed.Contains(otherAccountId))
                    Console.WriteLine("Error, List.Unfollow does not unfollow");
                if (followers.Contains(listIdThatExists))
                    Console.WriteLine("Error, List.Unfollow does not unfollow");
                storage.List.Follow(listIdThatExists, otherAccountId);
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Unfollow"); }
            Console.Write(".");

            #endregion

            #region HashSet<Guid> GetAccounts(Guid listId)

            //test exception ListNotFound

            try
            {
                storage.List.GetAccounts(new Guid());
                Console.WriteLine("Error, List.GetAccounts does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetAccounts"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                if (!storage.List.GetAccounts(listIdThatExists).Contains(storage.Account.GetId("accountThatExists")))
                    Console.WriteLine("Error, List.GetAccounts does not give good accounts");
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetAccounts"); }
            Console.Write(".");

            #endregion

            #region void Add(Guid listId, Guid accountId)

            //test exception ListNotFound

            try
            {
                storage.List.Add(Guid.NewGuid(), storage.Account.GetId("accountThatExists"));
                Console.WriteLine("Error, List.Add does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Add"); }
            Console.Write(".");

            //test exception AccountNotFound

            try
            {
                storage.List.Add(listIdThatExists, Guid.NewGuid());
                Console.WriteLine("Error, List.Add does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Add"); }
            Console.Write(".");

            //test normal beahaviour : already tested

            #endregion

            #region void Remove(Guid listId, Guid accountId)

            //test normal beahaviour

            try
            {
                Guid accountThatExistsId = storage.Account.GetId("accountThatExists");
                storage.List.Remove(listIdThatExists, accountThatExistsId);
                HashSet<Guid> accounts = storage.List.GetAccounts(listIdThatExists);
                if (accounts.Contains(accountThatExistsId))
                    Console.WriteLine("Error, List.Remove does not remove");
                storage.List.Add(listIdThatExists, accountThatExistsId);
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.Remove"); }
            Console.Write(".");

            #endregion

            #region HashSet<Guid> GetAccountOwnedLists(Guid accountId, bool withPrivate)

            //test exception AccountNotFound

            try
            {
                storage.List.GetAccountOwnedLists(Guid.NewGuid(), false);
                Console.WriteLine("Error, List.GetAccountOwnedLists does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetAccountOwnedLists"); }
            Console.Write(".");

            try
            {
                storage.List.GetAccountOwnedLists(Guid.NewGuid(), true);
                Console.WriteLine("Error, List.GetAccountOwnedLists does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetAccountOwnedLists"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                HashSet<Guid> ownedLists = storage.List.GetAccountOwnedLists(storage.Account.GetId("accountThatExists"), false);
                if (!ownedLists.Contains(listIdThatExists))
                    Console.WriteLine("GetAccountOwnedLists does not return the expected result");
            }
            catch (Exception e) { Console.WriteLine("Unhandled exception in List.GetAccountOwnedLists : " + e.ToString()); }
            Console.Write(".");

            #endregion

            #region HashSet<Guid> GetAccountFollowedLists(Guid accountId, bool withPrivate)

            //test exception AccountNotFound

            try
            {
                storage.List.GetAccountFollowedLists(new Guid(), false);
                Console.WriteLine("Error, List.GetAccountFollowedLists does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetAccountFollowedLists"); }
            Console.Write(".");

            try
            {
                storage.List.GetAccountFollowedLists(new Guid(), true);
                Console.WriteLine("Error, List.GetAccountFollowedLists does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetAccountFollowedLists"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                HashSet<Guid> followedList = storage.List.GetAccountFollowedLists(storage.Account.GetId("otherAccountThatExists"), false);
                if (!followedList.Contains(listIdThatExists))
                    Console.WriteLine("Error, GetAccountFollowedLists does not return the expected result");
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetAccountFollowedLists"); }
            Console.Write(".");

            #endregion

            #region HashSet<Guid> GetFollowingLists(Guid accountId);

            //test exception AccountNotFound

            try
            {
                storage.List.GetFollowingLists(Guid.NewGuid());
                Console.WriteLine("Error, List.GetFollowingLists does not raise AccountNotFound");
            }
            catch (AccountNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetFollowingLists"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                HashSet<Guid> followingLists = storage.List.GetFollowingLists(storage.Account.GetId("accountThatExists"));
                if (!followingLists.Contains(listIdThatExists))
                    Console.WriteLine("Error, GetFollowingLists does not return the expected result");
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetFollowingLists"); }
            Console.Write(".");

            #endregion

            #region HashSet<Guid> GetFollowingAccounts(Guid listId)

            //test exception ListNotFound

            try
            {
                storage.List.GetFollowingAccounts(new Guid());
                Console.WriteLine("Error, List. GetFollowingAccounts does not raise ListNotFound");
            }
            catch (ListNotFound) { }
            catch (Exception) { Console.WriteLine("Unhandled exception in List. GetFollowingAccounts"); }
            Console.Write(".");

            //test normal beahaviour

            try
            {
                HashSet<Guid> followingAccounts = storage.List.GetFollowingAccounts(listIdThatExists);
                if (!followingAccounts.Contains(storage.Account.GetId("accountThatExists")))
                    Console.WriteLine("Error, GetFollowingAccounts does return the expected result");
            }
            catch (Exception) { Console.WriteLine("Unhandled exception in List.GetFollowingAccounts"); }
            Console.Write(".");

            #endregion

            Console.WriteLine();
        }

        static void TestMessages(IStorage storage)
        {
            Console.WriteLine("Testing Messages");
            Guid account1 = storage.Account.GetId("accountThatExists");
            Guid account2 = storage.Account.GetId("otherAccountThatExists");
            Guid list = storage.List.GetPersonalList(account2);
            HashSet<Guid> lists1 = new HashSet<Guid>();
            lists1.Add(list);

            for (int i = 0; i < 100; i++)
            {
                storage.Msg.Post(account2, "message " + i);
                Console.Write(".");
            }

            foreach (var m in storage.Msg.GetListsMsgTo(lists1, DateTime.MaxValue, 60))
                Console.WriteLine(m.Content);

            Console.WriteLine();
        }

        static void ClearContainer(CloudBlobContainer c)
        {
            BlobRequestOptions opt = new BlobRequestOptions();
            opt.UseFlatBlobListing = true;
            foreach (CloudBlob blob in c.ListBlobs(opt))
                blob.Delete();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Clearing previous data");
            BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
            blobFactory.InitStorage();

            Console.WriteLine("Init connexions");
            Storage storage = new Storage(azureAccountName, azureAccountKey);

            Console.WriteLine("Filling with initial data");
            Guid userId = storage.User.Create("userThatExists", "userThatExists@gmail.com", new Byte[1]);
            Guid accountId = storage.Account.Create(userId, "accountThatExists", "accountThatExistsDesc");
            storage.User.Create("otherUserThatExists", "otherUserThatExists@gmail.com", new Byte[1]);
            Guid otherAccountId = storage.Account.Create(userId, "otherAccountThatExists", "otherAccountThatExistsDesc");

            Guid listId = storage.List.Create(storage.Account.GetId("accountThatExists"), "listThatExists", "Yeah", false);
            storage.List.Add(listId, accountId);
            storage.List.Add(listId, otherAccountId);
            storage.List.Follow(listId, accountId);
            storage.List.Follow(listId, otherAccountId);

            Console.WriteLine("Init ok");

            //TestUser(storage);
            //TestAccounts(storage);
            //TestList(storage, listId);

            TestMessages(storage);

            storage.Msg.GetTaggedFrom(accountId, DateTime.MinValue, 100);

            Console.ReadLine();

        }
    }
}
