using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using StorageLibrary;

namespace Tigwi_API.Controllers
{
    public class ApiController : Controller
    {
        //
        // GET: /usertimeline/{name}/{numberOfMessages}

        public FileStreamResult UserTimeline(string name, int numberOfMessages = 20)
        {
            IStorage storage = new Storage("",""); // connexion

            var id = storage.User.GetId(name);
            var listMsgs = storage.Msg.GetListsMsgTo(new HashSet<int> {id}, int.MaxValue, numberOfMessages);

            var stream = new MemoryStream();
            var result = new FileStreamResult(stream, "xml");

            var serialize = new XmlSerializer(typeof (List<IMessage>));
            serialize.Serialize(stream,listMsgs);

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
