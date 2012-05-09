using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tigwi.Storage.Library;

namespace Tigwi.Auth
{
    /// <summary>
    /// This exception is thrown when the authentication token is invalid
    /// </summary>

    public class AuthFailedException : Exception
    {
        public AuthFailedException() { }
    }

    /// <summary>
    /// Authenticate an user using an API key.
    /// </summary>
    public class ApiKeyAuth
    {
        private string apiKey;
        private IStorage storage;

        public ApiKeyAuth(IStorage storage, string apiKey)
        {
            this.apiKey = apiKey;
            this.storage = storage;
        }

        public Guid Authenticate()
        {
            // Pour l'instant, apikey == username
            try
            {
                Guid userId = this.storage.User.GetId(this.apiKey);
                return userId;
            }
            catch (UserNotFound e)
            {
                throw new AuthFailedException();
            }
        }
    }
}
