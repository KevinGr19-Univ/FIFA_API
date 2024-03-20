using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class NationManager : BaseRepository<Nation>, INationManager
    {
        public NationManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Nation?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
