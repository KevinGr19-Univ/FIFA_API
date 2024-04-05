using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Requête d'anonymisation de compte.
    /// </summary>
    public class AnonymizeRequest
    {
        /// <summary>
        /// Le mot de passe du compte.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// La raison de l'action.
        /// </summary>
        public string Reason { get; set; }
    }
}
