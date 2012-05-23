using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    public class ErrorController : HomeController
    {
        #region Http 404

        public ActionResult Http404(string url, HandleErrorInfo error)
        {
            var exception = error.Exception;
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            // If the url is relative ('NotFound' route), replace with Requested path
            var uri = this.Request.Url;
            var requestedUrl = uri != null && uri.OriginalString != url
                               && (url == null || uri.OriginalString.Contains(url))
                                   ? uri.OriginalString
                                   : url;

            // Don't get the user stuck in a 'retry loop' by allowing the Referrer to be the Request.
            var referrerUrl = this.Request.UrlReferrer != null && this.Request.UrlReferrer.OriginalString != requestedUrl
                                    ? this.Request.UrlReferrer.OriginalString
                                    : null;

            var model = new NotFoundViewModel { RequestedUrl = requestedUrl, ReferrerUrl = referrerUrl, Message = exception.Message };

            // TODO: log
            return this.View("NotFound", model);
        }

        public class NotFoundViewModel
        {
            public string RequestedUrl { get; set; }

            public string ReferrerUrl { get; set; }

            public string Message { get; set; }
        }

        #endregion

        #region Http 500

        public ActionResult Http500(string url, HandleErrorInfo error)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // TODO:
            return this.View("Error", error);
        }

        #endregion
    }
}