using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models
{
    public class Admin
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "Administrator";
    }
}
