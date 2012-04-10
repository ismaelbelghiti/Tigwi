using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tigwi.UI.Controllers
{
    public class PostController : Controller
    {
        /// <summary>
        /// Tags (favorites) a post.
        /// </summary>
        /// <returns></returns>
        public ActionResult Tag()
        {
            throw new NotImplementedException("PostController.Tag");
        }

        /// <summary>
        /// Untag (unfavorites) a post.
        /// </summary>
        /// <returns></returns>
        public ActionResult UnTag()
        {
            throw new NotImplementedException("PostController.UnTag");
        }

        /// <summary>
        /// Write a new post.
        /// </summary>
        /// <returns></returns>
        public ActionResult Write()
        {
            throw new NotImplementedException("PostController.Write");
        }

        /// <summary>
        /// Deletes a post.
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            throw new NotImplementedException("PostController.Delete");
        }

        /// <summary>
        /// Reply to a  post. The difference with <c>Write</c> is that it could be better integrated in the UI
        /// (see twitter).
        /// </summary>
        /// <returns></returns>
        public ActionResult Reply()
        {
            throw new NotImplementedException("PostController.Reply");
        }

        /// <summary>
        /// Repost something poster by another user (Retweets in Twitter).
        /// </summary>
        /// <returns></returns>
        /// TODO: Find another name. The term "Repost" has a negative connotation.
        public ActionResult Repost()
        {
            throw new NotImplementedException("PostController.Repost");
        }
    }
}
