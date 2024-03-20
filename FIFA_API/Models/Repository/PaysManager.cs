using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class PaysManager : BaseRepository<Pays>, IPaysManager
    {
        public PaysManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Pays?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
