using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class APITokenInfo
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
