using JobPortalApplication.Models.CompanyModel;
using JobPortalApplication.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
