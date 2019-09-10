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

        public MembershipExtension():base()
        {
           
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            base.IsValid(value, validationContext);

            var property = validationContext.ObjectType.GetProperty(UppercasePasswordErrorMessage);



            

            if (!Regex.IsMatch(((RegisterViewModel)validationContext.ObjectInstance).Password, "[A-Z]"))
            {
                return new ValidationResult(UppercasePasswordErrorMessage);
            }
            return null;


        }
    }

}
