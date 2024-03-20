using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class CompetitionManager : BaseRepository<Competition>, ICompetitionManager
    {
        public CompetitionManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Competition?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
