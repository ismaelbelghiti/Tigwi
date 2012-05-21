using System;
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
        public string ListName { get; set; }

        [Required]
        [Display(Name = "List Description")]
        public string ListDescription { get; set; }

        [Required]
        [Display(Name = "Make list public :")]
        public bool ListPublic { get; set; }

        [Required]
        public IEnumerable<string> AccountIds { get; set; }

        //Not required (list creation)
        public Guid ListId { get; set; }

    }
}