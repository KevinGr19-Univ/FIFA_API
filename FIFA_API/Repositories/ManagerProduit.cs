using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FIFA_API.Repositories
{
    public sealed class ManagerProduit : BaseVisibleManager<Produit>, IManagerProduit
    {
        public ManagerProduit(DbContext context) : base(context) { }

        public async Task<Produit?> GetByIdWithAll(int id)
        {
            return await DbSet.Include(p => p.Variantes).ThenInclude(v => v.Stocks)
                .Include(p => p.Variantes).ThenInclude(v => v.Couleur)
                .Include(p => p.Tailles)
                .SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
