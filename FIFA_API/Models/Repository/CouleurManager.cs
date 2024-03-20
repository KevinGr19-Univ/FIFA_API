using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class CouleurManager : BaseRepository<Couleur>, ICouleurManager
    {
        public CouleurManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Couleur?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
