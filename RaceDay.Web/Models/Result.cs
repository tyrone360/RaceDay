
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class Result
    {
        public int ResultID { get; set; }

        [Required]
        public int EnrollmentID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Position must be greater than zero.")]
        public int Position { get; set; }
    }
}
