using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalApplication.Models.Users
{
    public class Userdto
    {
        [ForeignKey("UserModel")]

        public int? UserId { get; set; }
        public UserModel UserModel { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        [DataType(DataType.Date)]

        public DateTime Dob { get; set; }
        [Required(ErrorMessage = "MartialStatus is required")]

        public string MartialStatus { get; set; }
        [Required(ErrorMessage = "Category is required")]

        public string Category { get; set; }
        [Required(ErrorMessage = "Address is required")]

        public string Address { get; set; }
        [Required(ErrorMessage = "Language is required")]

        public string Language { get; set; }
        [Required(ErrorMessage = "Hometown is required")]

        public string Hometown { get; set; }
        [Required(ErrorMessage = "Pincode is required")]
        public int Pincode { get; set; }

        public int? Course { get; set; }
        public string? Duration { get; set; }
        public string Grade { get; set; }
        public string Skills { get; set; }
        public IFormFile? File { get; set; }




    }
}
