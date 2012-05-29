#region copyright
// Copyright (c) 2012, TIGWI
// All rights reserved.
// Distributed under  BSD 2-Clause license
#endregion
namespace Tigwi.UI.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RegisterViewModel
    {
        [Key]
        [Required(ErrorMessage = "Username Required")]
        [Display(Name = "Username")]
        [AllowHtml]
        public string Login { get; set; }

        [Required(ErrorMessage = "Email adress Required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$",ErrorMessage = "Please enter a valid email")]
        [AllowHtml]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 20")]
        [Display(Name = "Password")]
        [AllowHtml]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm password")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember me ?")]
        public bool RememberMe { get; set; }
    }
}