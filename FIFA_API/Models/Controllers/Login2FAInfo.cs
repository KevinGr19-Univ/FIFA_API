using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class Login2FAInfo
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
