using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tigwi.UI.Models.Storage;

namespace Tigwi.UI.Models
{
    public class EditListViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "List name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "List Description")]
        public string Description { get; set; }

        [Required]
        public IEnumerable<string> AccountIds { get; set; }

    }
}