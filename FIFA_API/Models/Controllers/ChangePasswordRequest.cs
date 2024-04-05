using FIFA_API.Models.Utils;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Requête de réinitialisation de mot de passe.
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// L'adresse mail du compte cible.
        /// </summary>
        [Required]
        public string Mail { get; set; }

        /// <summary>
        /// Le nouveau mot de passe du compte.
        /// </summary>
        [Required]
        [RegularExpression(ModelUtils.REGEX_PASSWORD, ErrorMessage = "Le mot de passe ne respecte pas toutes les règles")]
        public string NewPassword { get; set; }
    }
}
