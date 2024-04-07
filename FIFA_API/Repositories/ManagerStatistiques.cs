using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerStatistiques : BaseManager<Statistiques>, IManagerStatistiques
    {
        public ManagerStatistiques(FifaDbContext context) : base(context) { }

        public async Task<Statistiques?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.IdJoueur == key);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.IdJoueur == key);
        }
    }
}
