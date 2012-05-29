#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tigwi.UI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class SearchViewModel
    {
        [AllowHtml]
        public string SearchString { get; set; }
    }
}