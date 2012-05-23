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