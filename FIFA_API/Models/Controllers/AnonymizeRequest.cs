using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class AnonymizeRequest
    {
        [Required]
        public string Password { get; set; }

        public string Reason { get; set; }
    }
}
