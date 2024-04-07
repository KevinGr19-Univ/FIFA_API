using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerAuth2FALogin : BaseManager<Auth2FALogin>, IManagerAuth2FALogin
    {
        public ManagerAuth2FALogin(DbContext context) : base(context) { }

        public async Task<Auth2FALogin?> GetById(int key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.IdUtilisateur == key);
        }

        public async Task<bool> Exists(int key)
        {
            return await DbSet.AnyAsync(e => e.IdUtilisateur == key);
        }
    }
}
