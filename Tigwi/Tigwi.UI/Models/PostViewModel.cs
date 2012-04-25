using System;

namespace Tigwi.UI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PostViewModel
    {
        public PostViewModel(string poster)
        {
            Poster = poster;
            Content =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum aliquet sollicitudin rhoncus. In hac habitasse platea dictumst volutpat.";
        }
        public virtual string Poster { get; private set; }

        [DataType(DataType.DateTime)]
        public virtual DateTime PostingDate { get; private set; }

        [DataType(DataType.MultilineText)]
        public virtual string Content { get; set; }

        public int color { get; set; }
    }
}