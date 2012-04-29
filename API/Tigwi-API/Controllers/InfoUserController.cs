using System;
using System.Collections.Generic;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public abstract class InfoUserController : ApiController
    {

        // parts of code that are common when accessing methods by login or by id

        protected Answer AnswerMainInfo(Guid userId)
        {
            var userInfo = Storage.User.GetInfo(userId);
            var userToReturn = new User(userInfo, userId);
            return new Answer(userToReturn);
        }


        protected Answer AnswerAuthorizedAccounts(Guid userId, int number)
        {
            var authorizedAccounts = Storage.User.GetAccounts(userId);
            var authorizedAccountsInList = new HashSet<Guid>();
            foreach (var followedList in authorizedAccounts)
            {
                authorizedAccountsInList.UnionWith(Storage.List.GetAccounts(followedList));
            }

            // Get as many subscriptions as possible (maximum: numberOfSubscriptions)
            var size = Math.Min(authorizedAccountsInList.Count, number);
            var authorizedAccountsToReturn = BuildAccountListFromGuidCollection(authorizedAccountsInList, size, Storage);

            return new Answer(authorizedAccountsToReturn);
        }
    }
}
