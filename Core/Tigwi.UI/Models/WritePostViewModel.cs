#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System.ComponentModel.DataAnnotations;
using Tigwi.UI.Models.Storage;

namespace Tigwi.UI.Models
{
    using System.Web.Mvc;

    public class WritePostViewModel
    {
        [Required]
        [StringLength(140)]
        [AllowHtml]
        public string Content { get; set; }
    }
}