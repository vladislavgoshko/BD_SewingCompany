using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SewingCompany.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect adress")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

    }
}
