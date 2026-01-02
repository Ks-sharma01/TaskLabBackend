namespace TaskLabBackend.Models.Api
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsExpired { get; set; }
    }
}
