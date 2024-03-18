using FIFA_API.Contracts;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUtilisateurRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginController(IConfiguration config, IUtilisateurRepository repository, IPasswordHasher passwordHasher)
        {
            _config = config;
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInfo loginInfo)
        {
            IActionResult response = Unauthorized();
            Utilisateur? user = await Authenticate(loginInfo.Mail, loginInfo.Password);

            if(user is not null)
            {
                var tokenString = GenerateJwtString(user);
                response = Ok(new{
                    token = tokenString
                });

                user.DerniereConnexion = DateTime.Now;
            }

            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody])

        private async Task<Utilisateur?> Authenticate(string email, string password)
        {
            Utilisateur? user = (await _repository.GetByEmailAsync(email)).Value;
            if (user is null) return null;

            bool passwordMatch = _passwordHasher.Verify(user.HashMotDePasse, password);
            return passwordMatch ? user : null;
        }

        private string GenerateJwtString(Utilisateur user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            if (user.Role is not null) claims.Add(new("role", user.Role));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
