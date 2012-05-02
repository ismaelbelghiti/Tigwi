using System.ComponentModel.DataAnnotations;
using Tigwi.UI.Models.Storage;

namespace Tigwi.UI.Models
{
    public class WritePostViewModel
    {
        [Required]
        [StringLength(100)]
        public string Content { get; set; }
    }
}