using FIFA_API.Contracts;
using FIFA_API.Contracts.Repository;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIFA_API.Controllers
{
    [Route("api")]
    [ApiController]
    public partial class LoginController : ControllerBase
    {
        private readonly IUtilisateurManager _manager;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginController(IUtilisateurManager repository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _manager = repository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<APITokenInfo>> Login([FromBody] LoginInfo loginInfo)
        {
            Utilisateur? user = await Authenticate(loginInfo.Mail, loginInfo.Password);

            if(user is not null)
            {
                return await LoginUser(user);
            }

            return Unauthorized();
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterInfo registerInfo)
        {
            if(await _manager.IsEmailTaken(registerInfo.Mail))
            {
                return BadRequest("Email taken");
            }

            Utilisateur newUser = new Utilisateur()
            {
                Mail = registerInfo.Mail,
                IdLangue = registerInfo.IdLangue,
                IdPays = registerInfo.IdPays,
                HashMotDePasse = _passwordHasher.Hash(registerInfo.Password)
            };

            await _manager.AddAsync(newUser);
            return Ok();
        }

        [HttpPost("Login/Refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<APITokenInfo>> Refresh([FromBody] APITokenInfo apiToken)
        {
            Utilisateur? user = await _tokenService.GetUserFromExpiredAsync(apiToken.AccessToken);
            if(user is null)
            {
                return NotFound();
            }

            if(user.RefreshToken != apiToken.RefreshToken)
            {
                return Forbid();
            }

            return await LoginUser(user);
        }

        private async Task<Utilisateur?> Authenticate(string email, string password)
        {
            Utilisateur? user = await _manager.GetByEmailAsync(email);
            if (user is null) return null;

            bool passwordMatch = _passwordHasher.Verify(user.HashMotDePasse, password);
            return passwordMatch ? user : null;
        }

        private async Task<APITokenInfo> LoginUser(Utilisateur user)
        {
            var apiToken = new APITokenInfo
            {
                AccessToken = _tokenService.GenerateAccessToken(user),
                RefreshToken = _tokenService.GenerateRefreshToken()
            };

            user.RefreshToken = apiToken.RefreshToken;
            user.DerniereConnexion = DateTime.Now;

            await _manager.SaveChangesAsync();
            return apiToken;
        }
    }
}
