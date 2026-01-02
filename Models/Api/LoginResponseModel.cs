namespace TaskLabBackend.Models.Api
{
    public class LoginResponseModel
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? AccessToken { get; set; }

        public int ExpiresInMin { get; set; }
    }
}
