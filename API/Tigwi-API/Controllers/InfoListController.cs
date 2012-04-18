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
        // GET : /infolist/subscriptions/{idOfList}/{number}

        public ActionResult Subscriptions(Guid idOfList, int number)
        {
            //TODO: Implement this
            throw new NotImplementedException();
        }

        //
        // GET : /infolist/subscribers/{idOfList}/{number}

        public ActionResult Subscribers(Guid idOfList, int number)
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
        // GET : /infolist/messages/{idOfList}/{number}

        public ActionResult Messages(Guid idOfList, int number)
        {
            //TODO: implement this
            throw new NotImplementedException();
        }

    }
}
