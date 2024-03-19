using FIFA_API.Models.Utils;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class RegisterInfo
    {
        [Required]
        [EmailAddress(ErrorMessage = "Le mail doit être une adresse valide")]
        public string Mail { get; set; }

        [Required]
        [RegularExpression(ModelUtils.REGEX_PASSWORD, ErrorMessage = "Le mot de passe doit suivre toutes les règles")]
        public string Password { get; set; }

        [Required]
        public int IdLangue { get; set; }

        [Required]
        public int IdPays { get; set; }

        // TODO: Champs facultatifs
    }
}
