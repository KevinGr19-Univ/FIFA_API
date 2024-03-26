using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class JoueurManager : BaseRepository<Joueur>, IJoueurManager
    {
        public JoueurManager(FifaDbContext dbContext) : base(dbContext) { }

        public async Task<Joueur?> GetByIdAsync(int id)
        {
            return await Includes(DbSet).FirstOrDefaultAsync(x => x.Id == id);
        }

        private IQueryable<Joueur> Includes(IQueryable<Joueur> query)
        {
            return query.Include(j => j.Trophees)
                .Include(j => j.FaqJoueurs)
                .Include(j => j.Stats);
        }
    }
}
