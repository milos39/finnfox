using finnfox.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace finnfox.Extensions
{
    public class EmailExtensionAttribute: ValidationAttribute
    {
        private string duplicateEmailErrorMessage;

        public string DuplicateEmailErrorMessage { get => duplicateEmailErrorMessage; set => duplicateEmailErrorMessage = value; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var allUsers = _context.Users.ToList();

            if (allUsers.Contains(_context.Users.Where(m => m.Email ==((RegisterViewModel)validationContext.ObjectInstance).Email).SingleOrDefault()) )
                return new ValidationResult(DuplicateEmailErrorMessage);           
            else
                return ValidationResult.Success;

           

        }
    }
}