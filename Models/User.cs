using System.ComponentModel.DataAnnotations;

namespace TaskLabBackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? Provider { get; set; } = null;

        public bool IsEmailVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }
}
