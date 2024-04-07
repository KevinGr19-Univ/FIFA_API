using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerStockProduit : BaseManager<StockProduit>, IManagerStockProduit
    {
        public ManagerStockProduit(DbContext context) : base(context) { }

        public async Task<StockProduit?> GetById(int idvariante, int idtaille)
        {
            return await DbSet.FindAsync(idvariante, idtaille);
        }

        public async Task<bool> Exists(int idvariante, int idtaille)
        {
            return await DbSet.AnyAsync(e => e.IdVCProduit == idvariante && e.IdTaille == idtaille);
        }

    }
}
