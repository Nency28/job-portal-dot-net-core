using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int? CourseType { get; set; }
    }
}
