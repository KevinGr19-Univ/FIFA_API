using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Modèle épuré de <see cref="Commande"/>, servant à afficher un aperçu d'une commande.
    /// </summary>
    public class ApercuCommande
    {
        public int Id { get; set; }
        public int IdTypeLivraison { get; set; }

        public string AdresseRue { get; set; }
        public string AdresseVille { get; set; }
        public string AdresseCodePostal { get; set; }

        public DateTime DateCommande { get; set; }
        public IEnumerable<StatusCommande> Status { get; set; }

        /// <summary>
        /// Retourne l'aperçu d'une commande.
        /// </summary>
        /// <param name="commande">La commande à utiliser.</param>
        /// <returns>L'aperçu de la commande.</returns>
        public static ApercuCommande FromCommande(Commande commande)
        {
            return new()
            {
                Id = commande.Id,
                IdTypeLivraison = commande.IdTypeLivraison,
                AdresseRue = commande.RueLivraison,
                AdresseVille = commande.VilleLivraison,
                AdresseCodePostal = commande.CodePostalLivraison,
                DateCommande = commande.DateCommande,
                Status = commande.Status
            };
        }
    }
}
