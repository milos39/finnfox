using finnfox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace finnfox.Extensions
{
    public class MembershipChangePasswordExtension: MembershipPasswordAttribute
    {
        private string uppercasePasswordErrorMessage;
        public string UppercasePasswordErrorMessage { get => uppercasePasswordErrorMessage; set => uppercasePasswordErrorMessage = value; }
        private string lowercasePasswordErrorMessage;
        public string LowercasePasswordErrorMessage { get => lowercasePasswordErrorMessage; set => lowercasePasswordErrorMessage = value; }
        private string numberPasswordErrorMessage;
        public string NumberPasswordErrorMessage { get => numberPasswordErrorMessage; set => numberPasswordErrorMessage = value; }


        public MembershipChangePasswordExtension() : base()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            base.IsValid(value, validationContext);

            var property = validationContext.ObjectType.GetProperty(UppercasePasswordErrorMessage);

            bool flag = false;

            if (((ChangePasswordViewModel)validationContext.ObjectInstance).NewPassword == null)
                return null;

            ValidationResult validationResult = new ValidationResult("");

            if (!Regex.IsMatch(((ChangePasswordViewModel)validationContext.ObjectInstance).NewPassword, "[A-Z]"))
            {
                validationResult.ErrorMessage += " " + uppercasePasswordErrorMessage;
                flag = true;
            }

            if (!Regex.IsMatch(((ChangePasswordViewModel)validationContext.ObjectInstance).NewPassword, "[a-z]"))
            {


                if (flag)
                {
                    lowercasePasswordErrorMessage = "i jednu cifru (0 - 9)";
                    validationResult.ErrorMessage += " " + lowercasePasswordErrorMessage;
                }
                else
                {
                    validationResult.ErrorMessage += lowercasePasswordErrorMessage;
                }
                flag = true;
            }

            if (!Regex.IsMatch(((ChangePasswordViewModel)validationContext.ObjectInstance).NewPassword, "[0-9]"))
            {
                if (flag)
                {
                    numberPasswordErrorMessage = "i jedan broj (0 - 9)";
                    validationResult.ErrorMessage += " " + numberPasswordErrorMessage;
                }
                else
                {
                    validationResult.ErrorMessage += numberPasswordErrorMessage;
                }



            }

            if (!string.IsNullOrEmpty(validationResult.ErrorMessage))
                return validationResult;
            else
                return null;


        }
    }
}