using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerStockProduit : IRepository<StockProduit>
    {
        Task<StockProduit?> GetById(int idvariante, int idtaille);
        Task<bool> Exists(int idvariante, int idtaille);
        Task<bool> Exists(int[] idsvariante, int idtaille);
    }
}
