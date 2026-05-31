using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskLabBackend.Models.Api
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Token { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsExpired { get; set; }


        
    }
}
