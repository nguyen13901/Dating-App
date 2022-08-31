using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Database.Entities
{
    public class RegisterDto
    {
        [Required]
        [StringLength(32)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}