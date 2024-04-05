using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Requête de suppression de compte.
    /// </summary>
    public class DeleteRequest
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
