using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Tigwi_API.Controllers
{
    public class ApiController : Controller
    {
        //
        // GET: /usertimeline/{name}/{numberOfMessages}

        public ContentResult UserTimeline(string name, int numberOfMessages = 20)
        {
            var id = get_account_id();
            var result = new ContentResult();
            result.Content = "";
            return result;
        }

        //
        // GET : /usersubscriptions/{name}/{numberOfSubscriptions}

        public ActionResult UserSubscriptionsList(string name, int numberOfSubscriptions =20)
        {
            throw new NotImplementedException();
        }

        //
        // GET : /usersubscribers/{name}/{numberOfSusbscribers}

        public ActionResult UserSubscribersList(string name, int numberOfSubscribers = 20)
        {
            throw new NotImplementedException();
        }

        //
        // POST : /write

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WritePost(/*objet convenable*/)
        {
            throw new NotImplementedException();
        }

        //
        // POST : /suscribe

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Suscribe(/*objet convenable*/)
        {
            throw new NotImplementedException();
        }


    }
}
