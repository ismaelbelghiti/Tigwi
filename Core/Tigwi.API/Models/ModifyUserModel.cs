using System;

namespace Tigwi.API.Models
{
    // Models for request bodies

    [Serializable]
    public class ChangeInfo
    {
        public Guid? UserId { get; set; }
        public string UserLogin { get; set; }
        public string Info { get; set; }
    }

    [Serializable]
    public class ChangePassword
    {
        public Guid? UserId { get; set; }
        public string UserLogin { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    [Serializable]
    public class CreateAccount
    {
        public Guid? UserId { get; set; }
        public string UserLogin { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
    }
}