#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System.Web.Mvc;
using Tigwi.API.Models;
using Tigwi.Auth;
using Tigwi.Storage.Library;
using System.Xml.Serialization;
using System;
using System.Linq;
using System.Web;


namespace Tigwi.API.Controllers
{
    public class UserController : ApiController
    {
        //
        // GET : /user/maininfo
        // The user you get the info depends on who you are according to authentication
        public ActionResult MainInfo()
        {
            Answer output;

            try
            {
                // Key must be sent in a cookie
                var keyCookie = Request.Cookies.Get("key");

                if (keyCookie == null)
                    output = new Answer(new Error("No key cookie was sent"));
                else
                {
                    var userId = (new ApiKeyAuth(Storage, new Guid(keyCookie.Value))).Authenticate();
                    var userInfo = Storage.User.GetInfo(userId);
                    var userToReturn = new User(userInfo, userId);
                    output = new Answer(userToReturn);
                }
            }
            catch (AuthFailedException)
            {
                output = new Answer(new Error("Authentication failed"));
            }
            catch (Exception exception)
            {
                // Result is an non-empty error XML element
                output = new Answer(HandleError(exception));
            }

            return Serialize(output);
        }


        //
        // POST : user/generatekey
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateKey()
        {
            Answer output;

            try
            {
                var idParameters = (Identity)(new XmlSerializer(typeof(Identity))).Deserialize(Request.InputStream);
                var userId = idParameters.UserId ?? Storage.User.GetId(idParameters.UserLogin);

                // We catch the real password to verify if both are the same
                var hashedPassword = Storage.User.GetPassword(userId);
                var otherHash = PasswordAuth.HashPassword(idParameters.Password);

                // If both passwords are the same, we can generate the key
                if (hashedPassword.SequenceEqual(otherHash)) 
                {
                    var key = Storage.User.GenerateApiKey(userId, idParameters.ApplicationName);
                    // We set a cookie but we also return the key in the response body
                    Response.SetCookie(new HttpCookie("key=" + key));
                    output = new Answer(new NewObject(key));
                }
                else
                    output = new Answer(new Error("Authentication failed"));
            }
            catch (UserNotFound)
            {
                output = new Answer(new Error("Authentication failed")); // Not a 404 error then
            }
            catch (Exception exception)
            {
                output = new Answer(HandleError(exception));
            }

            return Serialize(output);
        }
    }
}
