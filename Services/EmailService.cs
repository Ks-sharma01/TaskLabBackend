using SendGrid;
using System.Net;
using System.Net.Mail;

namespace TaskLabBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService( IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["EmailSettings:SmtpServer"],
                Port = Convert.ToInt32(_configuration["EmailSettings:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                    )
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:From"]),
                Subject = "Your OTP for TaskLab Login",
                Body = $@"
                        <h3>OTP Verification</h3>
                        <p>Your OTP is: <h2>{otp}</h2> </p>
                        <p>This OTP will expire in 5 minutes.</p>
                        ",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
