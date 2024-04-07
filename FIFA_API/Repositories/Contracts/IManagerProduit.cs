using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerProduit : IVisibleRepository<Produit>
    {
        Task<Produit?> GetByIdWithAll(int id);
    }
}
