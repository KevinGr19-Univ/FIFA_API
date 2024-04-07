using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerVarianteCouleurProduit : IVisibleRepository<VarianteCouleurProduit>
    {
        Task<VarianteCouleurProduit?> GetByIdWithData(int id, bool onlyVisible = true);
        Task<VarianteCouleurProduit?> GetByIdWithStocks(int id, bool onlyVisible = true);
        Task<bool> CombinationExists(int idproduit, int idcouleur);
    }
}
