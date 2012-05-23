using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using Tigwi.Storage.Library;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public class ListController : ApiController
    {
        //
        // GET : /list/subscriptions/{idOfList}/{number}

        public ActionResult Subscriptions(Guid idOfList, int number)
        {
            Answer output;

            try
            {
                // get accounts followed by the given list 
                var followedAccounts = Storage.List.GetAccounts(idOfList);

                var numberToReturn = Math.Min(number, followedAccounts.Count);
                var followedAccountsToReturn = AccountsFromGuidCollection(followedAccounts, numberToReturn, Storage);

                output = new Answer(followedAccountsToReturn);
            }

            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }

            return Serialize(output);
        }


        //
        // GET : /list/subscribers/{idOfList}/{number}

        public ActionResult Subscribers(Guid idOfList, int number)
        {
            Answer output;

            try
            {
                // get accounts following a given list 
                var listSuscriberAccounts = Storage.List.GetFollowingAccounts(idOfList);

                var numberToReturn = Math.Min(number, listSuscriberAccounts.Count);
                var listSuscribersOutputToReturn = AccountsFromGuidCollection(listSuscriberAccounts, numberToReturn, Storage);

                output = new Answer(listSuscribersOutputToReturn);
            }
            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }

            return Serialize(output);
        }


        //
        // GET : /list/owner/{idOfList}

        public ActionResult Owner(Guid idOfList)
        {
            Answer output;

            try
            {
                // get accounts following a given list 
                var ownerId = Storage.List.GetOwner(idOfList);
                var ownerInfo = Storage.Account.GetInfo(ownerId);
                var ownerToReturn = new Account(ownerId, ownerInfo.Name, ownerInfo.Description);
                output = new Answer(ownerToReturn);
            }

            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }

            return Serialize(output);
        }


        //
        // GET : /list/messages/{idOfList}/{number}

        public ActionResult Messages(Guid idOfList, int number)
        {
            Answer output;

            try
            {
                // get lasts messages from list defined by idOfList
                var listMsgs = Storage.Msg.GetListsMsgTo(new HashSet<Guid> { idOfList }, DateTime.Now, number);

                // convert, looking forward XML serialization
                var listMsgsOutput = new Messages(listMsgs, Storage);

                output = new Answer(listMsgsOutput);
            }
            catch (StorageLibException exception)
            {
                output = new Answer(new Error(exception.Code.ToString()));

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }

            return Serialize(output);
        }


        // Subscribe to the list
        // POST : /list/subscribe

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Subscribe()
        {
            Error error;

            try
            {
                var subscribe =
                    (Subscribe)(new XmlSerializer(typeof(Subscribe))).Deserialize(Request.InputStream);

                if (subscribe.AccountId == null && subscribe.AccountName == null)
                    error = new Error("AccountId or AccountName missing");
                else if (subscribe.List == null)
                    error = new Error("Subscription missing");
                else
                {
                    var accountId = subscribe.AccountId ?? Storage.Account.GetId(subscribe.AccountName);

                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(accountId);

                    if (authentication.HasRights)
                    {
                        Storage.List.Follow(subscribe.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }


        //
        // POST : /list/create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            Answer output;

            try
            {
                var listCreation =
                    (Create)(new XmlSerializer(typeof(Create))).Deserialize(Request.InputStream);

                if (listCreation.AccountId == null && listCreation.AccountName == null)
                    output = new Answer(new Error("AccountId or AccountName missing"));
                else if (listCreation.ListInfo == null)
                    output = new Answer(new Error("ListInfo missing"));
                else if (listCreation.ListInfo.Name == null)
                    output = new Answer(new Error("Name missing"));
                else if (listCreation.ListInfo.Description == null)
                    output = new Answer(new Error("Description missing"));
                else
                {
                    var accountId = listCreation.AccountId ?? Storage.Account.GetId(listCreation.AccountName);

                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(accountId);

                    if (authentication.HasRights)
                    {
                        var listToCreate = listCreation.ListInfo;

                        var listId = Storage.List.Create(accountId, listToCreate.Name, listToCreate.Description,
                                                         listToCreate.IsPrivate);

                        // Result is an empty error XML element
                        output = new Answer(new NewObject(listId));
                    }
                    else
                        output = new Answer(new Error(authentication.ErrorMessage()));
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }
            catch (InvalidOperationException exception)
            {
                output = new Answer(new Error(exception.Message + " " + exception.InnerException.Message));
            }

            return Serialize(output);
        }

        
        //
        // POST : /list/addaccount/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAccount()
        {
            Error error;

            try
            {
                var subscribeAccount =
                    (AddAccount)(new XmlSerializer(typeof(AddAccount))).Deserialize(Request.InputStream);

                if (subscribeAccount.AccountName == null && subscribeAccount.AccountId == null)
                    error = new Error("Account missing");
                else if (subscribeAccount.List == null)
                    error = new Error("List missing");
                else
                {
                    var ownerId = Storage.List.GetOwner(subscribeAccount.List.GetValueOrDefault());
                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(ownerId);

                    if (authentication.HasRights)
                    {
                        var accountId = subscribeAccount.AccountId ?? Storage.Account.GetId(subscribeAccount.AccountName);
                        Storage.List.Add(subscribeAccount.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }


        //
        // POST : /list/removeaccount/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveAccount()
        {
            Error error;

            try
            {
                var unsubscribeAccount =
                    (RemoveAccount)(new XmlSerializer(typeof(RemoveAccount))).Deserialize(Request.InputStream);

                if (unsubscribeAccount.AccountName == null && unsubscribeAccount.AccountId == null)
                    error = new Error("Account missing");
                else if (unsubscribeAccount.List == null)
                    error = new Error("List missing");
                else
                {
                    var ownerId = Storage.List.GetOwner(unsubscribeAccount.List.GetValueOrDefault());
                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(ownerId);

                    if (authentication.HasRights)
                    {
                        var accountId = unsubscribeAccount.AccountId ?? Storage.Account.GetId(unsubscribeAccount.AccountName);
                        Storage.List.Remove(unsubscribeAccount.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());
                }
            }
            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                error = new Error(exception.Code.ToString());

                // In the case of a "not found" exception we change the HTTP status
                if (exception.Code == StrgLibErr.MessageNotFound || exception.Code == StrgLibErr.ListNotFound || exception.Code == StrgLibErr.UserNotFound || exception.Code == StrgLibErr.AccountNotFound)
                    Response.StatusCode = 404;
            }
            catch (InvalidOperationException exception)
            {
                error = new Error(exception.Message + " " + exception.InnerException.Message);
            }

            return Serialize(new Answer(error));
        }
        

    }
}
