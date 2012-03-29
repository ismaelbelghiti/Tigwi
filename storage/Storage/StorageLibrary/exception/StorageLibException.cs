using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary.exception
{
    public class StorageLibException : Exception
    {
        public enum ErrCode
        {
            UserNotFound,
            AccountNotFound,
            ListNotFound,
            UserAlreadyExists,
            AccountAlreadyExist,
        }

        public StorageLibException(ErrCode code)
        {
            Code = code;
        }

        public ErrCode Code { get; private set; }
    }
}
