using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using finnfox.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace finnfox.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Trenutna lozinka")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} mora biti dugačka najmanje {2} slova.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        [MembershipExtension
            (
             MinRequiredPasswordLength = 6,
             MinNonAlphanumericCharactersError = "Nemate alfanumericke karaktere",
             ErrorMessage = "mora: biti dugacka najmanje {2} slova, sadrzati minimum jednu cifru ( 0 - 9) i mora imati bar jedno veliko slovo ( A - Z). ",
             MinPasswordLengthError = "Lozinka mora imati najmanje 6 slova",
             PasswordStrengthError = "Mora imati karaktere od a do z",
             UppercasePasswordErrorMessage = "Lozinka mora imati minimum jedno veliko slovo"

            )

            ]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrdite novu lozinku")]
        [Compare("NewPassword", ErrorMessage = "Lozinke se ne poklapaju")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}