using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        public string? ProfilePicture { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}