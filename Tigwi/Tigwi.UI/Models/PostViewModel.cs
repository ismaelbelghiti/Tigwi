using System;
using Tigwi.UI.Models.Storage;

namespace Tigwi.UI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PostViewModel
    {
        [Required]
        public virtual StorageAccountModel Poster { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public virtual DateTime PostingDate { get; set; }

        [Required]
        [StringLength(140)]
        [DataType(DataType.MultilineText)]
        public virtual string Content { get; set; }

        public int Color { get; set; }
    }
}