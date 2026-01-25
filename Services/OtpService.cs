using System.Security.Cryptography;
using System.Text;

namespace TaskLabBackend.Services
{
    public class OtpService : IOtpService
    {
        public string GenerateOtp()
        {
            var otp = new Random().Next(100000, 999999).ToString(); 
            return otp;
        }

        public string HashOtp(string otp)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(otp))
                );
        }

        public bool VerifyOtp(string otp,  string hash)
        {
            return HashOtp(otp) == hash;
        }
    }
}
