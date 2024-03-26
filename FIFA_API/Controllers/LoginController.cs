using FIFA_API.Contracts;
using FIFA_API.Models;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    [Route("api")]
    [ApiController]
    public partial class LoginController : ControllerBase
    {
        private readonly FifaDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginController(FifaDbContext dbContext, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _dbContext = dbContext;
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
            if(await _dbContext.Utilisateurs.IsEmailTaken(registerInfo.Mail))
            {
                return BadRequest("Email taken");
            }

            Utilisateur newUser = registerInfo.BuildUser(_passwordHasher);

            await _dbContext.AddAsync(newUser);
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

        [HttpGet("CheckLogin")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> CheckLogin()
        {
            return Ok();
        }

        private async Task<Utilisateur?> Authenticate(string email, string password)
        {
            Utilisateur? user = await _dbContext.Utilisateurs.GetByEmailAsync(email);
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

            await _dbContext.SaveChangesAsync();
            return apiToken;
        }
    }
}
