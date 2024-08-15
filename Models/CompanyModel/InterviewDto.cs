using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobPortalApplication.Models.Users;

namespace JobPortalApplication.Models.CompanyModel
{
    public class InterviewDto
    {
        public int InterviewId { get; set; }
        public int? UserId { get; set; }

       

        public int? JobId { get; set; }


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
