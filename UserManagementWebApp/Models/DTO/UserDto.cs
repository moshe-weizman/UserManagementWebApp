using System.ComponentModel.DataAnnotations;

namespace UserManagementWebApp.Models
{
    public class UserDto
    {
   
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

    }
}
