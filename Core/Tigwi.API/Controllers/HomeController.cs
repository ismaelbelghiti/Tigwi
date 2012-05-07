using System.Web.Mvc;

namespace Tigwi.API.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Your request is invalid";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
