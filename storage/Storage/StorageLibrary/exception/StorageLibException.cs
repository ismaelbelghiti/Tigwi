using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    // TODO : is this the best place

    public class AccountIsOwner : StorageLibException
    {
        public AccountIsOwner() : base(StrgLibErr.AccountIsOwner) { }
    }

    public class UserNotFound : StorageLibException
    {
        public UserNotFound() : base(StrgLibErr.UserNotFound) { }
    }

    public class AccountNotFound : StorageLibException
    {
        public AccountNotFound() : base(StrgLibErr.AccountNotFound) { }
    }

    public class ListNotFound : StorageLibException
    {
        public ListNotFound() : base(StrgLibErr.ListNotFound) { }
    }

    public class MessageNotFound : StorageLibException
    {
        public MessageNotFound() : base(StrgLibErr.MessageNotFound) { }
    }

    public class UserAlreadyExists : StorageLibException
    {
        public UserAlreadyExists() : base(StrgLibErr.UserAlreadyExists) { }
    }

    public class AccountAlreadyExists : StorageLibException
    {
        public AccountAlreadyExists() : base(StrgLibErr.AccountAlreadyExist) { }
    }

    public class UserIsAdmin : StorageLibException
    {
        public UserIsAdmin() : base(StrgLibErr.UserIsAdmin) { }
    }

    public class IsPersonnalList : StorageLibException
    {
        public IsPersonnalList() : base(StrgLibErr.IsPersonalList) { }
    }

    public class OpenIdUriNotAssociated : StorageLibException
    {
        public OpenIdUriNotAssociated() : base(StrgLibErr.OpenIdUriNotAssociated) { }
    }

    public class OpenIdUriDuplicated : StorageLibException
    {
        public OpenIdUriDuplicated() : base(StrgLibErr.OpenIdUriDuplicated) { }
    }

    // Deprecated
    public enum StrgLibErr
    {
        UserNotFound,
        AccountNotFound,
        ListNotFound,
        MessageNotFound,
        UserAlreadyExists,
        AccountAlreadyExist,
        UserIsAdmin,
        IsPersonalList,
        AccountIsOwner,
        OpenIdUriNotAssociated,
        OpenIdUriDuplicated,
    }

    // Deprecated
    [Serializable]
    public class StorageLibException : Exception
    {
        protected StorageLibException(StrgLibErr code)
        {
            Code = code;
        }

        public StrgLibErr Code { get; protected set; }
    }
}
