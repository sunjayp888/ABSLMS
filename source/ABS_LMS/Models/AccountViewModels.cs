using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ABS_LMS.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        
    }

    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please Enter Username")]
        [StringLength(50, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Username should be alphanumeric only")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "First Name: Please enter letters only")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Last Name: Please enter letters only")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    public class ResetPasswordViewModel
    {

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PasswordMessage { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public bool IsValidEmployee { get; set; }
    }
}
