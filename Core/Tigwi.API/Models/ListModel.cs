using System;

namespace Tigwi.API.Models
{
    // Models for request bodies
    [Serializable]
    public class ListAndAccount : BaseRequest
    {
        public Guid? List;
    }

    // Subscribe/Create list and Add/Remove account

    [Serializable]
    public class Subscribe : ListAndAccount { }

    [Serializable]
    public class AddAccount : ListAndAccount { }
    
    [Serializable]
    public class RemoveAccount : ListAndAccount { }

    [Serializable]
    public class ListInfo
    {
        public string Name;
        public string Description;
        public bool IsPrivate;
    }

    [Serializable]
    public class Create : BaseRequest
    {
        public ListInfo ListInfo { get; set; }
    }
}