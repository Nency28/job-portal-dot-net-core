using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models
{
    public class Industry
    {

        [Key]
        public int IndustryId { get; set; }
        public string IndustryName { get; set; }
        public int? Status { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
