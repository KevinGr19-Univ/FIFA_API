using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerAuthPasswordReset : BaseManager<AuthPasswordReset>, IManagerAuthPasswordReset
    {
        public ManagerAuthPasswordReset(FifaDbContext context) : base(context) { }

        public async Task<AuthPasswordReset?> GetById(string key)
        {
            return await DbSet.SingleOrDefaultAsync(e => e.Mail == key);
        }

        public async Task<bool> Exists(string key)
        {
            return await DbSet.AnyAsync(e => e.Mail == key);
        }
    }
}
