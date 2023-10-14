using boka.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace boka
{
    public class JwtAuthService : IJwtAuthService
    {
        private readonly List<User> Users = new List<User>()
        {
            new User
            {
                Id = 1,
                Username = "Kernel",
                Email = "kernel0748@gmail.com",
                Password = "testJwt"
            },
            new User
            {
                Id = 2,
                Username = "Boga",
                Email = "boga-roma@gmail.com",
                Password = "PwdUser"
            },
        };
        public User Auth(string email, string password)
        {
            return Users.Where(u => u.Email.ToUpper().Equals(email.ToUpper())
                && u.Password.Equals(password)).FirstOrDefault();
        }
        public string TokenGenerer(string secret, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
