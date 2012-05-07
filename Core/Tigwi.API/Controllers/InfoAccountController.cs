using System;
using System.Collections.Generic;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public abstract class InfoAccountController : ApiController
    {

        // parts of code that are common when accessing methods by name or by id

        protected Answer AnswerMessages(Guid accountId, int number)
        {
            // get lasts messages from account accoutName
            var personalListId = Storage.List.GetPersonalList(accountId);
            var listMsgs = Storage.Msg.GetListsMsgTo(new HashSet<Guid> { personalListId }, DateTime.Now, number);

            // convert, looking forward XML serialization
            var listMsgsOutput = new Messages(listMsgs, Storage);
            return new Answer(listMsgsOutput);
        }
        
        /*
        protected Answer AnswerTaggedMessages(Guid accountId, int number)
        {
            // get lasts messages from user name
            var listMsgs = Storage.Msg.GetTaggedTo(accountId, DateTime.Now, number);

            // convert, looking forward XML serialization
            var listMsgsOutput = new Messages(listMsgs, Storage);
            return new Answer(listMsgsOutput);
        }
        */
        

        protected Answer AnswerSubscriberAccounts(Guid accountId, int number)
        {
            // get lasts followers of user name 's list
            var followingLists = Storage.List.GetFollowingLists(accountId);
            var hashFollowers = new HashSet<Guid>();
            foreach (var followingList in followingLists)
            {
                hashFollowers.UnionWith(Storage.List.GetFollowingAccounts(followingList));
            }

            // Get as many subscribers as possible (maximum: number)
            var size = Math.Min(hashFollowers.Count, number);
            var accountListToReturn = BuildAccountListFromGuidCollection(hashFollowers, size, Storage);

            return new Answer(accountListToReturn);
        }

        
        protected Answer AnswerSubscriptionsEitherPublicOrAll(Guid accountId, int numberOfSubscriptions, bool withPrivate)
        {
            // get the public lists followed by the given account
            var followedLists = Storage.List.GetAccountFollowedLists(accountId, withPrivate);
            var accountsInLists = new HashSet<Guid>();
            foreach (var followedList in followedLists)
            {
                accountsInLists.UnionWith(Storage.List.GetAccounts(followedList));
            }

            // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
            var size = Math.Min(accountsInLists.Count, numberOfSubscriptions);
            var accountListToReturn = BuildAccountListFromGuidCollection(accountsInLists, size, Storage);

            return new Answer(accountListToReturn);
        }


        protected Answer AnswerSubscribedListsEitherPublicOrAll(Guid accountId, int numberofLists, bool withPrivate)
        {
            // get the public lists followed by the given account
            var followedLists = Storage.List.GetAccountFollowedLists(accountId, withPrivate);

            // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
            var size = Math.Min(followedLists.Count, numberofLists);
            var listsToReturn = BuildListsFromGuidCollection(followedLists, size, Storage);

            return new Answer(listsToReturn);
        }


        protected Answer AnswerSubscriberLists(Guid accountId, int numberOfSubscribers)
        {
            // get lasts followers of user name 's list
            var followingLists = Storage.List.GetFollowingLists(accountId);

            // Get as many subscribers as possible (maximum: number)
            var size = Math.Min(followingLists.Count, numberOfSubscribers);
            var accountListToReturn = BuildAccountListFromGuidCollection(followingLists, size, Storage);

            return new Answer(accountListToReturn);
        }


        protected Answer AnswerOwnedListsEitherPublicOrAll(Guid accountId, int numberOfLists, bool withPrivate)
        {
            // get the public lists owned by the given account
            var ownedLists = Storage.List.GetAccountOwnedLists(accountId, withPrivate);
            
            // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
            var size = Math.Min(ownedLists.Count, numberOfLists);
            var listsToReturn = BuildListsFromGuidCollection(ownedLists, size, Storage);

            return new Answer(listsToReturn);
        }


        protected Answer AnswerMainInfo(Guid accountId)
        {
            // get the informations of the given account
            var accountInfo = Storage.Account.GetInfo(accountId);
            var accountToReturn = new Account(accountId, accountInfo.Name, accountInfo.Description);
            return new Answer(accountToReturn);
        }

        /*
        // WARNING : The following methods may be a little too complicated and not necessary in an API
        protected Answer AnswerUsersAllowed(Guid accountId, int number)
        {
            // get users posting on this account
            var users = Storage.Account.GetUsers(accountId);

            // Get as many users as possible (maximum: number)
            var size = Math.Min(users.Count, number);
            var userListToReturn = BuilUserListFormGuidCollection(users, size, Storage);

            return new Answer(userListToReturn);
        }
        */
        
        /*
        protected Answer AnswerAdministrator(Guid accountId)
        {
            // get account's administrator
            var adminId = Storage.Account.GetAdminId(accountId);
            var adminInfo = Storage.User.GetInfo(adminId);
            var admin = new User(adminInfo, Storage.User.GetId(adminInfo.Login));

            return new Answer(admin);
        }
        */

    }
}
