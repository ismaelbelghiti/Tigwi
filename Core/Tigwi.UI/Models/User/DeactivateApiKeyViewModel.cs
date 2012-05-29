#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
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