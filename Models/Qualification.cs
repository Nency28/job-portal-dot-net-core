 using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models
{
    public class Qualification
    {

        [Key]
        public int QuaId { get; set; }
        public string QualificationName { get; set; }
        public int? CourseType { get; set; }
    }
}

