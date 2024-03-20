using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class ThemeVoteManager : BaseRepository<ThemeVote>, IThemeVoteManager
    {
        public ThemeVoteManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<ThemeVote?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
