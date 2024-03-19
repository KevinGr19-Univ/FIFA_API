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
            return await DbSet.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produit>> SearchProductsAsync(string query)
        {
            return await DbSet.Where(p => p.Titre.ToLower().Contains(query)).ToListAsync();
        }
    }
}
