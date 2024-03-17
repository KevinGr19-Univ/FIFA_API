using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class LoginInfo
    {
        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
