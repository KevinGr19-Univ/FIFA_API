using FIFA_API.Models.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWorkProduit : IUnitOfWork
    {
        IManagerCategorieProduit Categories { get; }
        IManagerProduit Produits { get; }
        IManagerVarianteCouleurProduit Variantes { get; }
        IManagerCouleur Couleurs { get; }
        IManagerTailleProduit Tailles { get; }
        IManagerCompetition Competitions { get; }
        IManagerGenre Genres { get; }
        IManagerNation Nations { get; }
        IManagerStockProduit Stocks { get; }

        Task<IEnumerable<SearchProductItem>> SearchProduits(
            string? q,
            int[] categories,
            int[] competitions,
            int[] genres,
            int[] nations,
            int[] couleurs,
            int[] tailles,
            bool? desc,
            int page,
            int amount);
    }
}
