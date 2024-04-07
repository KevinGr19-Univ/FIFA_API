using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerJoueur : BaseManager<Joueur>, IManagerJoueur
    {
        public ManagerJoueur(DbContext context) : base(context) { }

        public async Task<Joueur?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Id == key);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.Id == key);
        }

        public async Task<Joueur?> GetByIdWithData(int id)
        {
            return await DbSet.Include(j => j.Trophees).Include(j => j.FaqJoueurs)
                .Include(j => j.Club)
                .SingleOrDefaultAsync(j => j.Id == id);
        }

        public async Task<Joueur?> GetByIdWithPublications(int id)
        {
            return await DbSet.Include(j => j.Publications).SingleOrDefaultAsync(j => j.Id == id);
        }
    }
}
