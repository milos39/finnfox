using finnfox.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Web.Security;

namespace finnfox.Models
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
        [Display(Name = "UserName")]
        [EmailAddress]
        public string UserName{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Šifra")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Morate popuniti E-mail")]
        [EmailAddress(ErrorMessage ="Email nije u korektnom formatu")]
        [EmailExtension
            (
                DuplicateEmailErrorMessage = "Ova email adresa vec sadrzi nalog, pokusajte sa drugom adresom"
            )
        ]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Morate uneti ime  ")]
        [Display(Name = "Ime") ]
        public string UserName { get;set ; }


        [Display(Name = "Prezime")]
        [Required(ErrorMessage ="Morate uneti prezime ")]
        public string UserLastName { get; set; }

        //Passwords must have at least one non letter or digit character. Passwords must have at least one digit ('0'-'9'). Passwords must have at least one uppercase ('A'-'Z').

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora: biti dugacka najmanje {2} slova, sadrzati minimum jednu cifru ( 0 - 9) i mora imati bar jedno veliko slovo ( A - Z).", MinimumLength = 6)]
        //[DataType(DataType.Password, ErrorMessage = "Password nije u korektnom formatu")]
        [Display(Name = "Šifra")]
        [MembershipExtension
            (
             MinRequiredPasswordLength = 6,
             MinNonAlphanumericCharactersError = "Nemate alfanumericke karaktere",
             ErrorMessage = "mora: biti dugacka najmanje {2} slova, sadrzati minimum jednu cifru ( 0 - 9) i mora imati bar jedno veliko slovo ( A - Z). ",
             MinPasswordLengthError = "Password mora imati najmanje 6 slova",
             PasswordStrengthError ="Mora imati karaktere od a do z",
             UppercasePasswordErrorMessage = "Password mora imati minimum jedno veliko slovo"
            
            )
             
            ]
        public string Password { get; set; }

        [DataType(DataType.Password,ErrorMessage = "Šifra mora imati najmanje")]
        [Display(Name = "Potvrdite šifru")]
        [Compare("Password", ErrorMessage = "Šifre se ne poklapaju ")]
        public string ConfirmPassword { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
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
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
