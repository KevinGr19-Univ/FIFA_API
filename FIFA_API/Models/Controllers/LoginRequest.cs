using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Requête d'authentification.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// L'adresse mail du compte.
        /// </summary>
        [Required]
        public string Mail { get; set; }

        /// <summary>
        /// Le mot de passe du compte.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
