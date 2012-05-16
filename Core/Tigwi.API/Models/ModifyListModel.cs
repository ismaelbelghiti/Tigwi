using System;

namespace Tigwi.API.Models
{
    // Models for request bodies
    [Serializable]
    public class ListAndAccount
    {
        public Guid? List;
        public string AccountName { get; set; }
        public Guid? AccountId { get; set; }
    }
}