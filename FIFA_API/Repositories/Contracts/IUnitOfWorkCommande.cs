using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWorkCommande : IUnitOfWork
    {
        IManagerCommande Commandes { get; }
        IManagerProduit Produits { get; }
        IManagerCouleur Couleurs { get; }
        IManagerVarianteCouleurProduit Variantes { get; }
        IManagerTailleProduit Tailles { get; }
        IManagerStockProduit Stocks { get; }
        IManagerTypeLivraison TypeLivraisons { get; }
        IManagerUtilisateur Utilisateurs { get; }

        Task<CommandeDetails> GetDetails(Commande commande);
    }
}
