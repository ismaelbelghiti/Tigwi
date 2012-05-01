using System;

namespace Tigwi_API.Models
{
    // Models for request bodies

    [Serializable]
    public class ListAndAccount
    {
        public Guid List;
        public string Account;
    }
}