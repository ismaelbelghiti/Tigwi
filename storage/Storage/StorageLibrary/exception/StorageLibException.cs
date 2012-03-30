using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    // TODO : is this the best place
    public enum StrgLibErr
    {
        UserNotFound,
        AccountNotFound,
        ListNotFound,
        UserAlreadyExists,
        AccountAlreadyExist,
    }

    public class StorageLibException : Exception
    {
        public StorageLibException(StrgLibErr code)
        {
            Code = code;
        }

        public StrgLibErr Code { get; private set; }
    }
}
