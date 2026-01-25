namespace TaskLabBackend.Models.Api
{
    public class LoginRequestModel
    {
        public int UserId { get; set; }
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
