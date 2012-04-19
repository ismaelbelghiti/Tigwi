using System;
using System.Collections.Generic;
using System.Linq;
using StorageLibrary;
using System.Xml.Serialization;

namespace Tigwi_API.Models
{
    [Serializable]
    public class ListSubscribe
    {
        public Guid List;
        public string Subscription;
    }
}