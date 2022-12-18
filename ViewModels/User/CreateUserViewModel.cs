using System;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect adress")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Role")]
        public string UserRole { get; set; }
        public CreateUserViewModel()
        {
            UserRole = "user";
        }

    }
}