#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tigwi.Storage.Library
{
    public interface IStorage
    {
        IUserStorage User { get; }
        IAccountStorage Account { get; }
        IListStorage List { get; }
        IMsgStorage Msg { get; }
    }
}
