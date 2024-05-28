using System.ComponentModel.DataAnnotations;

namespace JobPortalApplication.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int? Status { get; set; }
    }
}
