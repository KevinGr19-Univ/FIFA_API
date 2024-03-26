using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class ClubManager : BaseRepository<Club>, IClubManager
    {
        public ClubManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Club?> GetByIdAsync(int id)
        {
            return await Includes(DbSet).FirstOrDefaultAsync(c => c.Id == id);
        }

        private IQueryable<Club> Includes(IQueryable<Club> query)
        {
            return query;
        }
    }
}
