using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerVarianteCouleurProduit : IVisibleRepository<VarianteCouleurProduit>
    {
        Task<VarianteCouleurProduit?> GetByIdWithAll(int id);
        Task<bool> CombinationExists(int idproduit, int idcouleur);
    }
}
