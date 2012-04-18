using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageLibrary;

namespace Tigwi.UI.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class PostModel
    {
        [Key, Editable(false)]
        public virtual Guid Id { get; protected set; }

        [Editable(false)]
        public virtual AccountModel Poster { get; protected set; }

        [DataType(DataType.DateTime)]
        public virtual DateTime PostingDate { get; protected set; }

        [DataType(DataType.MultilineText)]
        public virtual string Content { get; set; }

        internal abstract void TagBy(AccountModel account);

        internal abstract void UnTagBy(AccountModel account);
    }
}