using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class LangueManager : BaseRepository<Langue>, ILangueManager
    {
        public LangueManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Langue?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
