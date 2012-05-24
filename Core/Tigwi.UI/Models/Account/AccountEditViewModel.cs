using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Tigwi.UI.Models.Storage;

    public class AccountEditViewModel
    {
        [Required]
        [StringLength(140, MinimumLength = 0)]
        [Display(Name = "Description")]
        [AllowHtml]
        public string Description { get; set; }

        [Required]
        public IEnumerable<string> UserIds { get; set; }

        //Not required 
        public Guid AccountId { get; set; }

        //Not required
        [AllowHtml]
        public string ReturnController { get; set; }
        [AllowHtml]
        public string ReturnAction { get; set; }
    }
}