using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Linq;
using StorageLibrary;
using Tigwi_API.Models;

namespace Tigwi_API.Controllers
{
    public class InfoListController : ApiController
    {
        //
        // GET : /infolist/subscriptions/{idOfList}/{numberOfSubscriptions}

        public ActionResult Subscriptions(Guid idOfList, int numberOfSubscriptions)
        {
            //TODO: Implement this
            throw new NotImplementedException();
        }

        //
        // GET : /infolist/subscribers/{idOfList}/{numberOfSubscribers}

        public ActionResult Subscribers(Guid idOfList, int numberOfSubscribers)
        {
            //TODO: Implement this
            throw new NotImplementedException();
        }

        //
        // GET : /infolist/owner/{idOfList}

        public ActionResult Owner(Guid idOfList)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

        //
        // GET : /infolist/messages/{idOfList}/{numberOfMessages}

        public ActionResult Messages(Guid idOfList, int numberOfMessages)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

    }
}
