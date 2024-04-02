using FIFA_API.Authorization;
using FIFA_API.Contracts;
using FIFA_API.Models;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Services;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    [Route("api")]
    [ApiController]
    public partial class LoginController : ControllerBase
    {
        private readonly FifaDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILogin2FAService _login2FAService;

        public LoginController(FifaDbContext dbContext, IPasswordHasher passwordHasher, ITokenService tokenService, ILogin2FAService login2FAService)
        {
            _context = dbContext;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _login2FAService = login2FAService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginInfo)
        {
            Utilisateur? user = await Authenticate(loginInfo.Mail, loginInfo.Password);
            if (user is null) return Unauthorized();

            if (user.Login2FA) return await Send2FACodeAsync(user);

            return Ok(await LoginUser(user));
        }

        [HttpPost("login/2fa")]
        public async Task<ActionResult<APITokenInfo>> Login2FA([FromBody] Login2FAInfo loginInfo)
        {
            var user = await _login2FAService.AuthenticateAsync(loginInfo.Token, loginInfo.Code);
            if (user is null || !user.DoubleAuthentification) return Unauthorized();

            // Verify phone number
            if (user.DateVerif2FA is null)
            {
                user.DateVerif2FA = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return Ok(await LoginUser(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerInfo, [FromServices] IEmailVerificator emailVerificator)
        {
            if(await _context.Utilisateurs.IsEmailTaken(registerInfo.Mail))
            {
                return BadRequest(new
                {
                    Message = "Email taken"
                });
            }

            Utilisateur newUser = registerInfo.BuildUser(_passwordHasher);

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

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

        [HttpGet("2fa/resendverify")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> SendVerify2FA()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            if (user.Login2FA) return NoContent();
            if (user.Telephone is null || !user.DoubleAuthentification) return BadRequest();

            return await Send2FACodeAsync(user);
        }

        private async Task<Utilisateur?> Authenticate(string email, string password)
        {
            Utilisateur? user = await _context.Utilisateurs.GetByEmailAsync(email);
            if (user is null || user.Anonyme) return null;

            bool passwordMatch = _passwordHasher.Verify(user.HashMotDePasse, password);
            return passwordMatch ? user : null;
        }

        private async Task<IActionResult> Send2FACodeAsync(Utilisateur user)
        {
            string token = await _login2FAService.Send2FACodeAsync(user);
            return Ok(new { token });
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

            await _context.SaveChangesAsync();
            return apiToken;
        }
    }
}
