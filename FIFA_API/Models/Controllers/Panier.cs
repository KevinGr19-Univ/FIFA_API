using FIFA_API.Models.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    public class Panier
    {
        [Required] public List<PanierItem> Items { get; set; }
    }

    public class PanierItem
    {
        [Required] public int IdVCProduit { get; set; }
        [Required] public int IdTaille { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
        [Required] public int Quantite { get; set; }
    }
}
