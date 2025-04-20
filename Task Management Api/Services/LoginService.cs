using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_Management_Api.Interfaces;
using Task_Management_Api.Models;

namespace Task_Management_Api.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Login(LoginModel login)
        {
            if (CheckAdmin(login))
            {
                var token = GenerateJwtToken(login.UserName, "Admin");
                return token;
            }
            else if (CheckUser(login))
            {
                var token = GenerateJwtToken(login.UserName, "User");
                return token;
            }

            return "unauthorized";



        }
        private bool CheckAdmin(LoginModel login)
        {
            return ( login.UserName == "Admin" && login.Password == "Admin" );
        }

        private bool CheckUser(LoginModel login)
        {
            return ( login.UserName == "User" && login.Password == "User" );
        }

        private string GenerateJwtToken(string username , string Role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: new[] 
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, Role)

                },
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
