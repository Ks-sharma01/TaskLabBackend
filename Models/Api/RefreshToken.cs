using System.ComponentModel.DataAnnotations;

namespace TaskLabBackend.Models.Api
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }

        public int UserId { get; set; }

        public DateTime Expiry { get; set; }

        public bool IsExpired { get; set; }

        
    }
}
