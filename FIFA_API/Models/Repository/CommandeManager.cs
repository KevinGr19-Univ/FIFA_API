using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Repository
{
    public class CommandeManager : BaseRepository<Commande>, ICommandeManager
    {
        public CommandeManager(FifaDbContext dbContext) : base(dbContext) { }

        public override async Task<IEnumerable<Commande>> GetAllAsync()
        {
            return await Includes(DbSet).ToListAsync();
        }

        public async Task<Commande?> GetByIdAsync(int id)
        {
            return await Includes(DbSet).FirstOrDefaultAsync(c => c.Id == id);
        }

        private IQueryable<Commande> Includes(IQueryable<Commande> query)
        {
            return query.Include(c => c.Status);
        }
    }
}
