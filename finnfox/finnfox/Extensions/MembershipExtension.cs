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
    public class MembershipExtension : MembershipPasswordAttribute
    {
        private string uppercasePasswordErrorMessage;
        public string UppercasePasswordErrorMessage { get => uppercasePasswordErrorMessage; set => uppercasePasswordErrorMessage = value; }
        private string lowercasePasswordErrorMessage;
        public string LowercasePasswordErrorMessage { get => lowercasePasswordErrorMessage; set => lowercasePasswordErrorMessage = value; }
        private string numberPasswordErrorMessage;
        public string NumberPasswordErrorMessage { get => numberPasswordErrorMessage; set => numberPasswordErrorMessage = value; }


        public MembershipExtension():base()
        {
           
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            base.IsValid(value, validationContext);

            var property = validationContext.ObjectType.GetProperty(UppercasePasswordErrorMessage);

            bool flag = false;

            if (((RegisterViewModel)validationContext.ObjectInstance).Password == null)
                return null;

            ValidationResult validationResult = new ValidationResult("");

            if (!Regex.IsMatch(((RegisterViewModel)validationContext.ObjectInstance).Password, "[A-Z]"))
            {
                validationResult.ErrorMessage +=" "+ uppercasePasswordErrorMessage;
                flag = true;
            }

            if (!Regex.IsMatch(((RegisterViewModel)validationContext.ObjectInstance).Password, "[a-z]"))
            {
                

                if (flag)
                {
                    lowercasePasswordErrorMessage = "i jednu cifru (0 - 9)";
                    validationResult.ErrorMessage += " "+ lowercasePasswordErrorMessage;
                }
                else
                {
                    validationResult.ErrorMessage += lowercasePasswordErrorMessage;
                }
                flag = true;
            }

            if (!Regex.IsMatch(((RegisterViewModel)validationContext.ObjectInstance).Password, "[0-9]"))
            {
                if (flag)
                {
                    numberPasswordErrorMessage = "i jedan broj (0 - 9)";
                    validationResult.ErrorMessage += " " + numberPasswordErrorMessage;
                }
                else
                {
                    validationResult.ErrorMessage +=  numberPasswordErrorMessage;
                }



            }

            if (!string.IsNullOrEmpty(validationResult.ErrorMessage))
                return validationResult;
            else
                return null;


        }
    }

}
