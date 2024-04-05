using FIFA_API.Models.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Objet de résultat de recherche de produits.
    /// </summary>
    public class SearchProductItem
    {
        [Required] public int Id { get; set; }
        [Required] public string Titre { get; set; }
        [Required] public decimal Prix { get; set; }
        [Required] public int[] Couleurs { get; set; }
        [Required] public int[] Tailles { get; set; }
        [Required] public string? ImageUrl { get; set; }
    }
}
