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