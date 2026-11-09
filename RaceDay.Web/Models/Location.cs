
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class Location
    {
        public int LocationID { get; set; }

        [Required]
        public string LocationName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
