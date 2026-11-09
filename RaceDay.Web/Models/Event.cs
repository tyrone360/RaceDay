
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class Event
    {
        public int EventID { get; set; }

        [Required]
        public string EventName { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        public int? CategoryID { get; set; }

        public string? CategoryName { get; set; }

        public int? LocationID { get; set; }

        public string? LocationName { get; set; }

        public string? Address { get; set; }
    }
}
