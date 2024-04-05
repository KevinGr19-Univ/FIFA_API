using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Jetons d'accès à un compte.
    /// </summary>
    public class APITokenInfo
    {
        /// <summary>
        /// Le jeton JWT d'accès au compte.
        /// </summary>
        [Required]
        public string AccessToken { get; set; }

        /// <summary>
        /// Le jeton d'actualisation du compte.
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}
