using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models.Storage
{
    using StorageLibrary;

    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string login, UserAlreadyExists innerException)
            : base("This username is already taken.", innerException)
        {
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string login, UserNotFound innerException)
            : base("There is no user with the given login `" + login + "'.", innerException)
        {
        }

        public UserNotFoundException(Guid id, UserNotFound innerException)
            : base("There is no user with the given ID `" + id + "'.", innerException)
        {
        }
    }

    public class DuplicateAccountException : Exception
    {
        public DuplicateAccountException(string name, AccountAlreadyExists innerException)
            : base("There is already an account with the given name `" + name + "'.", innerException)
        {
        }
    }

    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string name, AccountNotFound innerException)
            : base("There is no account with the given name `" + name + "'.", innerException)
        {
        }
    }
}