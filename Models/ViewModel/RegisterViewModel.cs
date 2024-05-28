using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "User Name is required")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Minimum Length is 6 , must contain 1 Upercase , 1 Lowercase, 1 Special Character, 1 digit")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Phone No is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number")]

        public string? Mobile { get; set; }
        [Required(ErrorMessage = "Country is required")]

        public int Country { get; set; }
        [Required(ErrorMessage = "State is required")]

        public int State { get; set; }
        [Required(ErrorMessage = "City is required")]

        public int City { get; set; }
        [Required(ErrorMessage = "Company Name is required")]

       

        public int? utype { get; set; }

    }
}
