
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int EventID { get; set; }
    }
}
