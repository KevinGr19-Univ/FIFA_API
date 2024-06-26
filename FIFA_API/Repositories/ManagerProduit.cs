using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;
using FIFA_API.Models.Controllers;

namespace FIFA_API.Repositories
{
    public sealed class ManagerProduit : BaseVisibleManager<Produit>, IManagerProduit
    {
        public ManagerProduit(FifaDbContext context) : base(context) { }

        public async Task<Produit?> GetByIdWithTailles(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Variantes)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produit?> GetByIdWithVariantes(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Tailles)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produit?> GetByIdWithVariantesAndTailles(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Variantes)
                .Include(p => p.Tailles)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produit?> GetByIdWithAll(int id, bool onlyVisible = true)
        {
            return await Visibility(DbSet, onlyVisible).Include(p => p.Variantes).ThenInclude(v => v.Stocks)
                .Include(p => p.Variantes).ThenInclude(v => v.Couleur)
                .Include(p => p.Tailles)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<SearchProductItem>> SearchProduits(Func<IQueryable<Produit>, Task<IQueryable<SearchProductItem>>> query)
        {
            var finalQuery = await query(Visibility(DbSet, true));
            return await finalQuery.ToListAsync();
        }
       
    }
}
