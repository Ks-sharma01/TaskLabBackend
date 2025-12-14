using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskLabBackend.Db;
using TaskLabBackend.Models.Api;

namespace TaskLabBackend.Services
{
    public class JwtConfigure
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public JwtConfigure(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }

    public async Task<LoginResponseModel?> Authenticate (LoginRequestModel loginRequest)
        {
            if(string.IsNullOrWhiteSpace(loginRequest.UserName) ||  string.IsNullOrWhiteSpace(loginRequest.Password)) return null;

            var userAccount = await _context.Users.FirstOrDefaultAsync(x => x.Name == loginRequest.UserName && x.Password == loginRequest.Password);
            if(userAccount == null) return null;

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, loginRequest.UserName)
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accesstoken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseModel
            {
                AccessToken = accesstoken,
                UserName = loginRequest.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
            
        }
    }
}
