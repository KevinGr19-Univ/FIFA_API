using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class VarianteCouleurProduitManager : BaseRepository<VarianteCouleurProduit>, IVarianteCouleurProduitManager
    {
        public VarianteCouleurProduitManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<VarianteCouleurProduit?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
