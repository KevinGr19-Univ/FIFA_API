using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerTrophee : BaseManager<Trophee>, IManagerTrophee
    {
        public ManagerTrophee(FifaDbContext context) : base(context) { }

        public async Task<Trophee?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Id == key);
        }

        public async Task<Trophee?> GetByIdWithJoueurs(int id)
        {
            return await DbSet.Include(t => t.Joueurs)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.Id == key);
        }

    }
}
