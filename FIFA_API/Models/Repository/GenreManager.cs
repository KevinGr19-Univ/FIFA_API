using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class GenreManager : BaseRepository<Genre>, IGenreManager
    {
        public GenreManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
