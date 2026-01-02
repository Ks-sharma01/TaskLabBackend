using System.ComponentModel.DataAnnotations;

namespace TaskLabBackend.Dto
{
    public class RegisterDto
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
