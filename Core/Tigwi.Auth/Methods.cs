using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            catch ( UserNotFound )
            {
                throw new AuthFailedException();
            }
        }
    }

    /// <summary>
    /// Authenticate an user using a username and a password
    /// </summary>
    public class PasswordAuth
    {
        private string username;
        private string password;
        private IStorage storage;

        // Random salt
        private static byte[] salt = {0x68, 0xe9, 0x7b, 0x60, 0x27, 0xec, 0x83, 0x45,
                                      0x68, 0x69, 0xff, 0x24, 0x15, 0xb4, 0x8e, 0x2c,
                                      0x85, 0x4d, 0x7a, 0xb2, 0x4f, 0x72, 0xfc, 0x61,
                                      0x7b, 0xb8, 0x72, 0x8f, 0xc5, 0x2a, 0x30, 0x83 };

        public PasswordAuth(IStorage storage, string username, string password)
        {
            this.storage = storage;
            this.username = username;
            this.password = password;
        }

        public static byte[] HashPassword(string password)
        {
            Rfc2898DeriveBytes k = new Rfc2898DeriveBytes(password, PasswordAuth.salt);
            return k.GetBytes(32);
        }

        public Guid Authenticate()
        {
            try
            {
                Guid userId = this.storage.User.GetId(this.username);
                byte[] hashedPassword = this.storage.User.GetPassword(userId);
                byte[] otherHash = PasswordAuth.HashPassword(this.password);
                if ( ! hashedPassword.SequenceEqual(otherHash) )
                    throw new AuthFailedException();
                return userId;
            }
            catch ( UserNotFound )
            {
                throw new AuthFailedException();
            }
        }
    }
}
