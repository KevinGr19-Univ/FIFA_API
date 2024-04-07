using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerLigneCommande : BaseManager<LigneCommande>, IManagerLigneCommande
    {
        public ManagerLigneCommande(FifaDbContext context) : base(context) { }

        public async Task<LigneCommande?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Id == key);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.Id == key);
        }
    }
}
