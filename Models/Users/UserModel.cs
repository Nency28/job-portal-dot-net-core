using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalApplication.Models.Users
{
    public class UserModel
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

        public int CountryId { get; set; }



        public int? StateId { get; set; }

        

        public int? CityId { get; set; }


        public DateTime? EntryDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int? utype { get; set; }
        public int? Status { get; set; }

    }
}
