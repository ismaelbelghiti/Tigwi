namespace Tigwi.UI.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RegisterViewModel
    {
        [Key]
        [Required]
        [Display(Name = "Username")]
        [AllowHtml]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [AllowHtml]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Password")]
        [AllowHtml]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm password")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember me ?")]
        public bool RememberMe { get; set; }
    }
}