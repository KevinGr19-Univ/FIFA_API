using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class LoginRequest
    {
        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
