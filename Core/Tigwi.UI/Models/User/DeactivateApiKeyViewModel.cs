namespace Tigwi.UI.Models.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class DeactivateApiKeyViewModel
    {
        [Required]
        public Guid ApiKey { get; set; }
    }
}