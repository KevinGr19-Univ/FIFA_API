using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class DeleteRequest
    {
        [Required]
        public string Password { get; set; }

        public string Reason { get; set; }
    }
}
