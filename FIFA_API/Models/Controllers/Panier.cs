using FIFA_API.Models.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Requête de création de commande.
    /// </summary>
    public class Panier
    {
        /// <summary>
        /// Liste des produits à acheter.
        /// </summary>
        [Required] public List<PanierItem> Items { get; set; }

        /// <summary>
        /// Lien de redirection en cas de succès.
        /// </summary>
        [Required] public string SuccessUrl { get; set; }

        /// <summary>
        /// Lien de redirection en cas d'annulation.
        /// </summary>
        [Required] public string CancelUrl { get; set; }
    }

    /// <summary>
    /// Objet de <see cref="Panier"/>.
    /// </summary>
    public class PanierItem
    {
        [Required] public int IdVCProduit { get; set; }
        [Required] public int IdTaille { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
        [Required] public int Quantite { get; set; }
    }
}
