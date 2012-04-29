using System;
using System.Web.Mvc;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoAccountByIdController : InfoAccountController
    {

        //
        // GET: /infoaccount/messages/{accountId}/{number}
        public ActionResult Messages(Guid accountId, int number)
        {
            Answer output;

            try
            {
                output = AnswerMessages(accountId, number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET: /infoaccount/taggedmessages/{accountId}/{number}
        public ActionResult TaggedMessages(Guid accountId, int number)
        {
            Answer output;

            try
            {
                output = AnswerTaggedMessages(accountId, number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

        
        //
        // GET : /infoaccount/subscriberaccounts/{accountId}/{number}
        public ActionResult SubscriberAccounts(Guid accountId, int number)
        {
            Answer output;

            try
            {
                output = AnswerSubscriberAccounts(accountId, number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        // To be used in following methods
        private ContentResult SubscriptionsEitherPublicOrAll(Guid accountId, int numberOfSubscriptions, bool withPrivate)
        {
            Answer output;

            try
            {
                output = AnswerSubscriptionsEitherPublicOrAll(accountId, numberOfSubscriptions, withPrivate);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET : /infoaccount/publiclysubscribedaccounts/{accountId}/{number}
        public ActionResult PubliclySubscribedAccounts(Guid accountId, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountId, number, false);
        }


        //
        // GET : /infoaccount/subscribedaccounts/{accountId}/{number}
        // [Authorize]
        public ActionResult SubscribedAccounts(Guid accountId, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountId, number, true);
        }

        private ActionResult SubscribedListsEitherPublicOrAll(Guid accountId, int numberofLists, bool withPrivate)
        {
            Answer output;

            try
            {
                output = AnswerSubscribedListsEitherPublicOrAll(accountId, numberofLists, withPrivate);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

        
        //
        // GET : /infoaccount/subscribedpubliclists/{accountId}/{number}
        public ActionResult SubscribedPublicLists(Guid accountId, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountId, number, false);
        }

        
        //
        // GET : /infoaccount/subscribedlists/{accountId}/{number}
        //[Authorize]
        public ActionResult SubscribedLists(Guid accountId, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountId, number, true);
        }
        
        
        //
        // GET : /infoaccount/subscriberLists/{accountId}/{number}
        public ActionResult SubscriberLists(Guid accountId, int number)
        {
            Answer output;

            try
            {
                output = AnswerSubscriberLists(accountId, number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        // To be used in following methods
        private ActionResult OwnedListsEitherPublicOrAll(Guid accountId, int numberOfLists, bool withPrivate)
        {
            Answer output;

            try
            {
                output = AnswerOwnedListsEitherPublicOrAll(accountId, numberOfLists,
                                                           withPrivate);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET : infoaccount/ownedpubliclists/{accountId}/{number}
        public ActionResult OwnedPublicLists(Guid accountId, int number)
        {
            return OwnedListsEitherPublicOrAll(accountId, number, false);
        }

        
        //
        // GET : /infoaccount/ownedlists/{accountId}/{number}
        //[Authorize]
        public ActionResult OwnedLists(Guid accountId, int number)
        {
            return OwnedListsEitherPublicOrAll(accountId, number, true);
        }


        //
        // GET : /infoaccount/main/{accountId}
        public ActionResult MainInfo(Guid accountId)
        {
            Answer output;

            try
            {
                output = AnswerMainInfo(accountId);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET : /infoaccount/usersallowed/{accountId}/{number}
        //[Authorize] (?)
        public ActionResult UsersAllowed(Guid accountId, int number)
        {
            Answer output;

            try
            {
                output = AnswerUsersAllowed(accountId, number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }


        //
        // GET : infoaccount/administrator/{accountId}
        //[Authorize]
        public ActionResult Administrator(Guid accountId)
        {
            Answer output;

            try
            {
                output = AnswerAdministrator(accountId);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            return Serialize(output);
        }

    }
}
