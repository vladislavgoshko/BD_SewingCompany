using System.ComponentModel.DataAnnotations;

namespace SewingCompany.ViewModels.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect adress")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string UserRole { get; set; }
        public EditUserViewModel()
        {
            UserRole = "user";
        }
    }
}