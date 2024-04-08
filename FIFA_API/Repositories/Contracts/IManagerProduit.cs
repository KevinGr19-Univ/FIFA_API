using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerProduit : IVisibleRepository<Produit>
    {
        Task<Produit?> GetByIdWithTailles(int id, bool onlyVisible = true);
        Task<Produit?> GetByIdWithVariantes(int id, bool onlyVisible = true);
        Task<Produit?> GetByIdWithVariantesAndTailles(int id, bool onlyVisible = true);
        Task<Produit?> GetByIdWithAll(int id, bool onlyVisible = true);
        Task<IEnumerable<SearchProductItem>> SearchProduits(Func<IQueryable<Produit>, Task<IQueryable<SearchProductItem>>> query);
    }
}
