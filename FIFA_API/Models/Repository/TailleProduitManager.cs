using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class TailleProduitManager : BaseRepository<TailleProduit>, ITailleProduitManager
    {
        public TailleProduitManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<TailleProduit?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
