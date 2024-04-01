using FIFA_API.Authorization;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginInfo, [FromServices] ILogin2FAService login2FAService)
        {
            Utilisateur? user = await Authenticate(loginInfo.Mail, loginInfo.Password);

            if (user is null) return Unauthorized();

            if (user.DoubleAuthentification && user.Telephone is not null)
            {
                string token = await login2FAService.Send2FACodeAsync(user);
                return Ok(new { token });
            }

            return Ok(await LoginUser(user));
        }

        [HttpPost("login/2fa")]
        public async Task<ActionResult<APITokenInfo>> Login2FA([FromBody] Login2FAInfo loginInfo, [FromServices] ILogin2FAService login2FAService)
        {
            var user = await login2FAService.AuthenticateAsync(loginInfo.Token, loginInfo.Code);
            if (user is null) return Unauthorized();

            return Ok(await LoginUser(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerInfo, [FromServices] IEmailVerificator emailVerificator)
        {
            if(await _dbContext.Utilisateurs.IsEmailTaken(registerInfo.Mail))
            {
                return BadRequest(new
                {
                    Message = "Email taken"
                });
            }

            Utilisateur newUser = registerInfo.BuildUser(_passwordHasher);

            await _dbContext.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            await emailVerificator.SendVerificationAsync(newUser);
            return NoContent();
        }

        [HttpPost("login/refresh")]
        public async Task<ActionResult<APITokenInfo>> Refresh([FromBody] APITokenInfo apiToken)
        {
            Utilisateur? user = await _tokenService.GetUserFromExpiredAsync(apiToken.AccessToken);
            if(user is null || user.Anonyme)
            {
                return NotFound();
            }

            if(user.RefreshToken != apiToken.RefreshToken)
            {
                return Forbid();
            }

            return await LoginUser(user);
        }

        [HttpGet("checklogin")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> CheckLogin()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            return user is null ? Unauthorized() : Ok();
        }

        [HttpGet("email/checkverified")]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<IActionResult> CheckEmailVerified()
        {
            return Ok();
        }

        [HttpGet("email/sendverify")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> SendEmailVerification([FromServices] IEmailVerificator emailVerificator)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            await emailVerificator.SendVerificationAsync(user);
            return NoContent();
        }

        [HttpPost("email/verify")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string code, [FromServices] IEmailVerificator emailVerificator)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            bool ok = await emailVerificator.VerifyAsync(user, code);
            return ok ? NoContent() : BadRequest();
        }

        [HttpGet("password/sendreset")]
        public async Task<IActionResult> SendResetPassword([FromQuery] string mail, [FromServices] IPasswordResetService resetService)
        {
            await resetService.SendPasswordResetCodeAsync(mail);
            return NoContent();
        }

        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword([FromQuery] string code, [FromBody] ChangePasswordRequest request, [FromServices] IPasswordResetService resetService)
        {
            bool ok = await resetService.ChangePasswordAsync(request, code);
            return ok ? NoContent() : BadRequest();
        }

        private async Task<Utilisateur?> Authenticate(string email, string password)
        {
            Utilisateur? user = await _dbContext.Utilisateurs.GetByEmailAsync(email);
            if (user is null || user.Anonyme) return null;

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
