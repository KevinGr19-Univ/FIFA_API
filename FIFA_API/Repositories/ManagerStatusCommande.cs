using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerStatusCommande : BaseManager<StatusCommande>, IManagerStatusCommande
    {
        public ManagerStatusCommande(DbContext context) : base(context) { }

        public async Task<StatusCommande?> GetById(int idcommande, CodeStatusCommande code)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.IdCommande == idcommande && e.Code == code);
        }

        public async Task<bool> Exists(int idcommande, CodeStatusCommande code)
        {
            return await DbSet.AnyAsync(e => e.IdCommande == idcommande && e.Code == code);
        }
    }
}
