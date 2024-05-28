using JobPortalApplication.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalApplication.Models.CompanyModel
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        [ForeignKey("Company")]

        public int? CompanyId { get; set; }
        public Company Company { get; set; }



        public int? UserId { get; set; }


        public string? JobTitle { get; set; }
        public string? Description { get; set; }
        public int? Position { get; set; }
        public int? PositionCompleted { get; set; }

        public string? QuaId { get; set; }
        public int? Course { get; set; }

        public string? Timing { get; set; }
        public string? Address { get; set; }
        public int? Industry { get; set; }
    
        public  string? WorkMode { get; set; }
        public string? WorkType { get; set; }
        public string? Salary { get; set; }

        public DateTime? EntryDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string? Gender { get; set; }
        public string? Experience { get; set; }
        public string? InterviewMode { get; set; }
        public string? Language { get; set; }


    }
}
