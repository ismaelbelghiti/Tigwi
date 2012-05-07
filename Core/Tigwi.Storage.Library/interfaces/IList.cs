using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tigwi.Storage.Library
{
    public interface IListInfo
    {
        string Name { get; set; }
        string Description { get; set; }
        bool IsPrivate { get; set; }
        bool IsPersonnal { get; set; }
    }
}
