using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Informations d'authentification 2FA.
    /// </summary>
    public class Login2FAInfo
    {
        /// <summary>
        /// Le jeton de 2FA associé au compte.
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Le code d'authentification 2FA.
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}
