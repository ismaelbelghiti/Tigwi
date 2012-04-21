using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tigwi.UI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    [DisplayColumn("Login")]
    public abstract class UserModel
    {
        public abstract Guid Id { get; }

        public abstract string Login { get; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public abstract string Email { get; set; }

        // TODO: abstract avatar type
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Avatar")]
        public abstract string Avatar { get; set; }

        public abstract ICollection<AccountModel> Accounts { get; }

        internal abstract void Save();
    }
}