using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Controllers
{
    public class CommandeDetails
    {
        public Commande Commande { get; set; }

        public Dictionary<int, string> Produits { get; set; }
        public Dictionary<int, string> Couleurs { get; set; }
        public Dictionary<int, string> Tailles { get; set; }

        public static async Task<CommandeDetails> FromCommande(Commande commande, FifaDbContext context)
        {
            var details = new CommandeDetails()
            {
                Commande = commande,
                Produits = new(),
                Couleurs = new(),
                Tailles = new()
            };

            foreach (var ligne in commande.Lignes)
            {
                var variante = await context.VarianteCouleurProduits.GetByIdAsync(ligne.IdVCProduit);
                details.Produits.Add(variante.IdProduit, variante.Produit.Titre);
                details.Couleurs.Add(variante.IdCouleur, variante.Couleur.Nom);

                var taille = await context.TailleProduits.FindAsync(ligne.IdTaille);
                details.Tailles.Add(taille.Id, taille.Nom);
            }

            return details;
        }
    }
}
