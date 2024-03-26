using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class TropheeManager : BaseRepository<Trophee>, ITropheeManager
    {
        public TropheeManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Trophee?> GetByIdAsync(int id)
        {
            return await Includes(DbSet).FirstOrDefaultAsync(t => t.Id == id);
        }

        private IQueryable<Trophee> Includes(IQueryable<Trophee> query)
        {
            return query;
        }
    }
}
