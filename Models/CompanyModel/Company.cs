using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalApplication.Models.CompanyModel
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [ForeignKey("UserModel")] 

        public int? UserId { get; set; }
        JobPortalApplication.Models.Users.UserModel UserModel { get; set; }

        [Required(ErrorMessage = "Company Name is required")]

        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Industry Name is required")]
        public int Industry { get; set; }
        [ForeignKey("Industry")]
        public Industry industry { get; set; }

        [Required(ErrorMessage = "Website is required")]
        public string Website { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]

        public string Mobile { get; set; }
        [Required(ErrorMessage = "Description is required")]

        public string Description { get; set; }
        public string Image { get; set; }
        public int? Status { get; set; }

    }
}
