using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalApplication.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]

        public Country Country { get; set; }

        public int StateId { get; set; }

        [ForeignKey("StateId")]

        public State State { get; set; }
        public int? Status { get; set; }
    }
}
