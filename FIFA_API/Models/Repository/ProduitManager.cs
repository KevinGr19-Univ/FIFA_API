using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class ProduitManager : BaseRepository<Produit>, IProduitManager
    {
        public ProduitManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Produit?> GetByIdAsync(int id)
        {
            return await IncludeDetails(DbSet).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produit>> SearchProductsAsync(string query)
        {
            return await IncludeDetails(
                DbSet.Where(p => p.Titre.ToLower().Contains(query))
            ).ToListAsync();
        }

        private IQueryable<Produit> IncludeDetails(IQueryable<Produit> query)
        {
            return query.Include(p => p.Variantes).Include(p => p.Tailles);
        }
    }
}
