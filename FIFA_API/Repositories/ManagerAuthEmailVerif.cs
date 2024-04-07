using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerAuthEmailVerif : BaseManager<AuthEmailVerif>, IManagerAuthEmailVerif
    {
        public ManagerAuthEmailVerif(DbContext context) : base(context) { }

        public async Task<AuthEmailVerif?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.IdUtilisateur == key);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.IdUtilisateur == key);
        }
    }
}
