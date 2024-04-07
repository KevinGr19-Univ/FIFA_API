using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Modèle épuré de <see cref="EntityFramework.Commande"/>, servant à afficher les détails d'une commande.
    /// </summary>
    public class CommandeDetails
    {
        public Commande Commande { get; set; }

        public Dictionary<int, string> Produits { get; set; }
        public Dictionary<int, string> Couleurs { get; set; }
        public Dictionary<int, string> Tailles { get; set; }
    }
}
