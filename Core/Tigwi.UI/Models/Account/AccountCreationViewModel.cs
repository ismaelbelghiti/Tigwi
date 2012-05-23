using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Tigwi.UI.Models.Storage;

    public class AccountCreationViewModel
    {

        [Required]
        [Display(Name = "Name")]
        [AllowHtml]
        public string Name { get; set; }

        [Required]
        [StringLength(140, MinimumLength = 0)]
        [Display(Name = "Description")]
        [AllowHtml]
        public string Description { get; set; }
    }
}