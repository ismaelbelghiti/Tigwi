#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using Tigwi.API.Models;

namespace Tigwi.API.Controllers
{
    public class ListController : ApiController
    {
        //
        // GET : /list/subscriptions/{idOfList}/{number}

        public ActionResult Subscriptions(Guid? idOfList, int number)
        {
            Answer output;

            try
            {
                if (idOfList == null)
                {
                    output = new Answer(new Error("List id missing"));
                    Response.StatusCode = 400; // Bad Request
                }
                else
                {
                    // get accounts followed by the given list 
                    var followedAccounts = Storage.List.GetAccounts(idOfList.GetValueOrDefault());

                    var numberToReturn = Math.Min(number, followedAccounts.Count);
                    var followedAccountsToReturn = AccountsFromGuidCollection(followedAccounts, numberToReturn, Storage);

                    output = new Answer(followedAccountsToReturn);
                }
            }

            catch (Exception exception)
            {
                output = new Answer(HandleError(exception));
            }

            return Serialize(output);
        }


        //
        // GET : /list/subscribers/{idOfList}/{number}

        public ActionResult Subscribers(Guid? idOfList, int number)
        {
            Answer output;

            try
            {
                if (idOfList == null)
                {
                    output = new Answer(new Error("List id missing"));
                    Response.StatusCode = 400; // Bad Request
                }
                else
                {
                    // get accounts following a given list 
                    var listSuscriberAccounts = Storage.List.GetFollowingAccounts(idOfList.GetValueOrDefault());

                    var numberToReturn = Math.Min(number, listSuscriberAccounts.Count);
                    var listSuscribersOutputToReturn = AccountsFromGuidCollection(listSuscriberAccounts, numberToReturn,
                                                                                  Storage);

                    output = new Answer(listSuscribersOutputToReturn);
                }
            }
            catch (Exception exception)
            {
                output = new Answer(HandleError(exception));
            }

            return Serialize(output);
        }


        //
        // GET : /list/owner/{idOfList}

        public ActionResult Owner(Guid? idOfList)
        {
            Answer output;

            try
            {
                if (idOfList == null)
                {
                    output = new Answer(new Error("List id missing"));
                    Response.StatusCode = 400; // Bad Request
                }
                else
                {
                    // get accounts following a given list 
                    var ownerId = Storage.List.GetOwner(idOfList.GetValueOrDefault());
                    var ownerInfo = Storage.Account.GetInfo(ownerId);
                    var ownerToReturn = new Account(ownerId, ownerInfo.Name, ownerInfo.Description);
                    output = new Answer(ownerToReturn);
                }
            }

            catch (Exception exception)
            {
                output = new Answer(HandleError(exception));
            }

            return Serialize(output);
        }


        //
        // GET : /list/messages/{idOfList}/{number}

        public ActionResult Messages(Guid? idOfList, int number)
        {
            Answer output;

            try
            {
                if (idOfList == null)
                {
                    output = new Answer(new Error("List id missing"));
                    Response.StatusCode = 400; // Bad Request
                }
                else
                {
                    // get lasts messages from list defined by idOfList
                    var listMsgs = Storage.Msg.GetListsMsgTo(new HashSet<Guid> {idOfList.GetValueOrDefault()},
                                                             DateTime.Now, number);

                    // convert, looking forward XML serialization
                    var listMsgsOutput = new Messages(listMsgs, Storage);

                    output = new Answer(listMsgsOutput);
                }
            }
            catch (Exception exception)
            {
                output = new Answer(HandleError(exception));
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
                {
                    error = new Error("AccountId or AccountName missing");
                    Response.StatusCode = 400; // Bad Request
                }
                else if (subscribe.List == null)
                {
                    error = new Error("List missing");
                    Response.StatusCode = 400; // Bad Request
                }
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
            catch (Exception exception)
            {
                // Result is an non-empty error XML element
                error = HandleError(exception);
            }

            return Serialize(new Answer(error));
        }


        // Unsubscribe from the list
        // POST : /list/subscribe

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Unsubscribe()
        {
            Error error;

            try
            {
                var unsubscribe =
                    (Unsubscribe)(new XmlSerializer(typeof(Unsubscribe))).Deserialize(Request.InputStream);

                if (unsubscribe.AccountId == null && unsubscribe.AccountName == null)
                {
                    error = new Error("AccountId or AccountName missing");
                    Response.StatusCode = 400; // Bad Request
                }
                else if (unsubscribe.List == null)
                {
                    error = new Error("List missing");
                    Response.StatusCode = 400; // Bad Request
                }
                else
                {
                    var accountId = unsubscribe.AccountId ?? Storage.Account.GetId(unsubscribe.AccountName);

                    // Check if the user is authenticated and has rights
                    var authentication = Authorized(accountId);

                    if (authentication.HasRights)
                    {
                        Storage.List.Unfollow(unsubscribe.List.GetValueOrDefault(), accountId);

                        // Result is an empty error XML element
                        error = new Error();
                    }
                    else
                        error = new Error(authentication.ErrorMessage());
                }
            }
            catch (Exception exception)
            {
                // Result is an non-empty error XML element
                error = HandleError(exception);
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
                {
                    output = new Answer(new Error("AccountId or AccountName missing"));
                    Response.StatusCode = 400; // Bad Request
                }
                else if (listCreation.ListInfo == null)
                {
                    output = new Answer(new Error("ListInfo missing"));
                    Response.StatusCode = 400; // Bad Request
                }
                else if (listCreation.ListInfo.Name == null)
                {
                    output = new Answer(new Error("Name missing"));
                    Response.StatusCode = 400; // Bad Request
                }
                else if (listCreation.ListInfo.Description == null)
                {
                    output = new Answer(new Error("Description missing"));
                    Response.StatusCode = 400; // Bad Request
                }
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
            catch (Exception exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(HandleError(exception));
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
                {
                    error = new Error("Account missing");
                    Response.StatusCode = 400; // Bad Request
                }
                else if (subscribeAccount.List == null)
                {
                    error = new Error("List missing");
                    Response.StatusCode = 400; // Bad Request
                }
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
            catch (Exception exception)
            {
                // Result is an non-empty error XML element
                error = HandleError(exception);
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
                {
                    error = new Error("Account missing");
                    Response.StatusCode = 400; // Bad Request
                }
                else if (unsubscribeAccount.List == null)
                {
                    error = new Error("List missing");
                    Response.StatusCode = 400; // Bad Request
                }
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
            catch (Exception exception)
            {
                // Result is an non-empty error XML element
                error = HandleError(exception);
            }

            return Serialize(new Answer(error));
        }
        

    }
}
