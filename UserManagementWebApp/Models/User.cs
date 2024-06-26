using System.ComponentModel.DataAnnotations;

namespace UserManagementWebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string Photo { get; set; }//path
    }

}
