using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageLibrary
{
    public interface IStorage
    {
        IUserStorage User { get; }
        IAccountStorage Account { get; }
        IListStorage List { get; }
        IMsgStorage Msg { get; }
    }
}
