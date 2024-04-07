using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerProduit : IVisibleRepository<Produit>
    {
        Task<Produit?> GetByIdWithTailles(int id);
        Task<Produit?> GetByIdWithVariantes(int id);
        Task<Produit?> GetByIdWithVariantesAndTailles(int id);
        Task<Produit?> GetByIdWithAll(int id);
        Task<IEnumerable<SearchProductItem>> SearchProduits(Func<IQueryable<Produit>, Task<IQueryable<SearchProductItem>>> query);
    }
}
