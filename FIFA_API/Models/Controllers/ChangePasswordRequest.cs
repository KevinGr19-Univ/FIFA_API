using FIFA_API.Models.Utils;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Mail { get; set; }

        [Required]
        [RegularExpression(ModelUtils.REGEX_PASSWORD, ErrorMessage = "Le mot de passe ne respecte pas toutes les règles")]
        public string NewPassword { get; set; }
    }
}
