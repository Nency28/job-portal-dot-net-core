using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models.Users
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Minimum Length is 8, must contain 1 Uppercase, 1 Lowercase, 1 Special Character, 1 Digit")]
        public string NewPassword { get; set; }
    }
}
