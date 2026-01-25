using System.ComponentModel.DataAnnotations;

namespace TaskLabBackend.Models
{
    public class OtpRequest
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public string OtpHash { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
