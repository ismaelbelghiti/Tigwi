using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;
using Tigwi_API.Models;


namespace Tigwi_API.Controllers
{
    public class InfoAccountByNameController : InfoAccountController
    {
        //
        // GET: /infoaccount/messages/{accountName}/{number}
        public ActionResult Messages(string accountName, int number)
        {
            Answer output;

            try
            {
                output = AnswerMessages(Storage.Account.GetId(accountName), number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }


        //
        // GET: /infoaccount/taggedmessages/{accountName}/{number}
        public ActionResult TaggedMessages(string accountName, int number)
        {
            Answer output;

            try
            {
                output = AnswerTaggedMessages(Storage.Account.GetId(accountName), number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : /infoaccount/subscriberaccounts/{accountName}/{number}
        public ActionResult SubscriberAccounts(string accountName, int number)
        {
            Answer output;

            try
            {
                output = AnswerSubscriberAccounts(Storage.Account.GetId(accountName), number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        // To be used in following methods
        private ContentResult SubscriptionsEitherPublicOrAll(string accountName, int numberOfSubscriptions, bool withPrivate)
        {
            Answer output;

            try
            {
                output = AnswerSubscriptionsEitherPublicOrAll(Storage.Account.GetId(accountName), numberOfSubscriptions, withPrivate);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }


        //
        // GET : /infoaccount/publiclysubscribedaccounts/{accountName}/{number}
        public ActionResult PubliclySubscribedAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/subscribedaccounts/{accountName}/{number}

        // [Authorize]
        public ActionResult SubscribedAccounts(string accountName, int number)
        {
            return SubscriptionsEitherPublicOrAll(accountName, number, true);
        }
        // To be used in following methods
        private ActionResult SubscribedListsEitherPublicOrAll(string accountName, int numberofLists, bool withPrivate)
        {
            Answer output;

            try
            {
                output = AnswerSubscribedListsEitherPublicOrAll(Storage.Account.GetId(accountName), numberofLists, withPrivate);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : /infoaccount/subscribedpubliclists/{accountName}/{number}
        public ActionResult SubscribedPublicLists(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/subscribedlists/{accountName}/{number}

        //[Authorize]
        public ActionResult SubscribedLists(string accountName, int number)
        {
            return SubscribedListsEitherPublicOrAll(accountName, number, true);
        }

        //
        // GET : /infoaccount/subscriberLists/{accountName}/{number}
        public ActionResult SubscriberLists(string accountName, int number)
        {
            Answer output;

            try
            {
                output = AnswerSubscriberLists(Storage.Account.GetId(accountName), number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        // To be used in following methods
        private ActionResult OwnedListsEitherPublicOrAll(string accountName, int numberOfLists, bool withPrivate)
        {
            Answer output;

            try
            {
                output = AnswerOwnedListsEitherPublicOrAll(Storage.Account.GetId(accountName), numberOfLists,
                                                           withPrivate);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }
        //
        // GET : infoaccount/ownedpubliclists/{accountName}/{number}
        public ActionResult OwnedPublicLists(string accountName, int number)
        {
            return OwnedListsEitherPublicOrAll(accountName, number, false);
        }

        //
        // GET : /infoaccount/ownedlists/{accountName}/{number}

        //[Authorize]
        public ActionResult OwnedLists(string accountName, int number)
        {
            return OwnedListsEitherPublicOrAll(accountName, number, true);
        }

        //
        // GET : /infoaccount/main/{accountName}

        //[Authorize]
        public ActionResult MainInfo(string accountName)
        {
            Answer output;

            try
            {
                output = AnswerMainInfo(Storage.Account.GetId(accountName));
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : /infoaccount/usersallowed/{accountName}/{number}
        //[Authorize] (?)
        public ActionResult UsersAllowed(string accountName, int number)
        {
            Answer output;

            try
            {
                output = AnswerUsersAllowed(Storage.Account.GetId(accountName), number);
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }

        //
        // GET : infoaccount/administrator/{accountName}

        //[Authorize]
        public ActionResult Administrator(string accountName)
        {
            Answer output;

            try
            {
                output = AnswerAdministrator(Storage.Account.GetId(accountName));
            }

            catch (StorageLibException exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(new Error(exception.Code.ToString()));
            }

            // a stream is needed for serialization
            var stream = new MemoryStream();
            (new XmlSerializer(typeof(Answer))).Serialize(stream, output);

            return Content(stream.ToString());
        }




    }
}
