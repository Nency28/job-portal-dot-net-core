using JobPortalApplication.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models.CompanyModel
{
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }
        public int? JobId { get; set; }
        public int? UserId { get; set; }
        public Job Job { get; set; }
        public UserModel User { get; set; }
        public int? ApplicationStatus { get; set; }
        public int? EntryStatus { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
