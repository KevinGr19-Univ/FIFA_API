using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FIFA_API.Services
{
    /// <summary>
    /// Service d'authentification par jetons JWT.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IManagerUtilisateur _manager;

        /// <summary>
        /// Crée une instance de <see cref="TokenService"/>.
        /// </summary>
        /// <param name="config">La configuration contenant les variables liées aux JWTs.</param>
        /// <param name="manager">Le manager à utiliser.</param>
        public TokenService(IConfiguration config, IManagerUtilisateur manager)
        {
            _config = config;
            _manager = manager;
        }

        public string GenerateAccessToken(Utilisateur user)
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

        public string GenerateRefreshToken()
        {
            byte[] buffer = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }

        public async Task<Utilisateur?> GetUserFromPrincipalAsync(ClaimsPrincipal principal)
        {
            var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim is null || !int.TryParse(idClaim.Value, out int userId)) return null;

            return await _manager.GetById(userId);
        }

        public async Task<Utilisateur?> GetUserFromExpiredAsync(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtToken = (JwtSecurityToken)securityToken;

            if (jwtToken is null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Access token is invalid");

            return await GetUserFromPrincipalAsync(principal);
        }
    }
}
