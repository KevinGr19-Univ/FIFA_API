using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Models.Controllers
{
    public class ApercuCommande
    {
        public int Id { get; set; }

        public string AdresseRue { get; set; }
        public string AdresseVille { get; set; }
        public string AdresseCodePostal { get; set; }

        public StatusCommande DernierStatus { get; set; }

        public static ApercuCommande FromCommande(Commande commande)
        {
            return new()
            {
                Id = commande.Id,
                AdresseRue = commande.RueLivraison,
                AdresseVille = commande.VilleLivraison,
                AdresseCodePostal = commande.CodePostalLivraison,
                DernierStatus = commande.Status.OrderByDescending(s => s.Date).First()
            };
        }
    }
}
