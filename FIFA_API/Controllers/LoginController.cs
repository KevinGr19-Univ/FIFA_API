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

        /// <summary>
        /// Authentifie un utilisateur.
        /// </summary>
        /// <param name="loginInfo">Les informations de connexion.</param>
        /// <returns>Les jetons d'accès au compte (<see cref="APITokenInfo"/>), ou un jeton d'authentification 2FA (<see cref="string"/>).</returns>
        /// <response code="401">Identifiants invalides.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginInfo)
        {
            Utilisateur? user = await Authenticate(loginInfo.Mail, loginInfo.Password);
            if (user is null) return Unauthorized();

            if (user.Login2FA) return await Send2FACodeAsync(user);

            return Ok(await LoginUser(user));
        }

        /// <summary>
        /// Authentifie un utilisateur avec un code de 2FA.
        /// </summary>
        /// <param name="loginInfo">Les informations de connexion 2FA.</param>
        /// <returns>Les jetons d'accès au compte (<see cref="APITokenInfo"/>).</returns>
        /// <response code="401">Identifiants invalides.</response>
        [HttpPost("login/2fa")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Crée un utilisateur.
        /// </summary>
        /// <param name="registerInfo">Les informations de création de compte.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="400">Les informations de création de compte sont invalides.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Regénère les jetons de connexion au compte, à partir d'anciens.
        /// </summary>
        /// <param name="apiToken">Les jetons de connexion au compte.</param>
        /// <returns>Les nouveaux jetons de connexion.</returns>
        /// <response code="404">L'utilisateur n'existe pas ou est désactivé.</response>
        /// <response code="403">Les jetons sont invalides.</response>
        [HttpPost("login/refresh")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Retourne l'état de la connexion à l'API.
        /// </summary>
        /// <returns>L'état de la connexion actuelle.</returns>
        /// <response code="401">Non-authentifié.</response>
        /// <response code="200">Authentifié.</response>
        [HttpGet("checklogin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> CheckLogin()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            return user is null ? Unauthorized() : Ok();
        }

        /// <summary>
        /// Retourne l'état de vérification de l'email de l'utilisateur.
        /// </summary>
        /// <returns>L'état de vérification de l'email.</returns>
        /// <response code="401">Non-authentifié ou email non-vérifié.</response>
        /// <response code="200">Email vérifié.</response>
        [HttpGet("email/checkverified")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<IActionResult> CheckEmailVerified()
        {
            return Ok();
        }

        /// <summary>
        /// Envoie un email de vérification à l'utilisateur pour vérifier son adresse mail.
        /// </summary>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet("email/sendverify")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> SendEmailVerification([FromServices] IEmailVerificator emailVerificator)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            await emailVerificator.SendVerificationAsync(user);
            return NoContent();
        }

        /// <summary>
        /// Vérifie l'adresse email de l'utilisateur avec un code.
        /// </summary>
        /// <param name="code">Le code de vérification envoyé.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le code est invalide.</response>
        [HttpPost("email/verify")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string code, [FromServices] IEmailVerificator emailVerificator)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            bool ok = await emailVerificator.VerifyAsync(user, code);
            return ok ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Envoie un mail de réinitialisation de mot de passe.
        /// </summary>
        /// <param name="mail">L'adresse mail de l'utilisateur.</param>
        /// <returns>Réponse HTTP</returns>
        [HttpGet("password/sendreset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendResetPassword([FromQuery] string mail, [FromServices] IPasswordResetService resetService)
        {
            await resetService.SendPasswordResetCodeAsync(mail);
            return NoContent();
        }

        /// <summary>
        /// Réinitialise le mot de passe de l'utilisateur lié à l'adresse mail.
        /// </summary>
        /// <param name="code">Le code de réinitialisation de mot de passe.</param>
        /// <param name="request">L'adresse mail et le nouveau mot de passe.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="400">Le code ou l'adresse mail sont invalides.</response>
        [HttpPost("password/reset")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ResetPassword([FromQuery] string code, [FromBody] ChangePasswordRequest request, [FromServices] IPasswordResetService resetService)
        {
            bool ok = await resetService.ChangePasswordAsync(request, code);
            return ok ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Envoie un SMS de vérification de 2FA à l'utilisateur.
        /// </summary>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="204">Le numéro de téléphone est déjà vérifié.</response>
        [HttpGet("2fa/resendverify")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
