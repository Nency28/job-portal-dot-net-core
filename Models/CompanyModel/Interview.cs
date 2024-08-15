using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobPortalApplication.Models.Users;


namespace JobPortalApplication.Models.CompanyModel
{
    public class Interview
    {
        [Key]
        public int InterviewId { get; set; }
        [Required(ErrorMessage = "The User field is required.")]

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
        [Required(ErrorMessage = "The Job field is required.")]

        public int? JobId { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        [DataType(DataType.Date)]

        public DateTime? InterviewDate { get; set; }
        public TimeSpan? Time { get; set; }

        public string? Location { get; set; }

        public int? Status { get; set; }
        [DataType(DataType.Date)]

        public DateTime? EntryDate { get; set; }
        [DataType(DataType.Date)]

        public DateTime? UpdateDate { get; set; }
        public string? Remark { get; set; }


    }
}
