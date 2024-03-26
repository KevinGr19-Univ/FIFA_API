using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class FaqJoueurManager : BaseRepository<FaqJoueur>, IFaqJoueurManager
    {
        public FaqJoueurManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<FaqJoueur?> GetByIdAsync(int id)
        {
            return await Includes(DbSet).FirstOrDefaultAsync(f => f.Id == id);
        }

        private IQueryable<FaqJoueur> Includes(IQueryable<FaqJoueur> query)
        {
            return query;
        }
    }
}
