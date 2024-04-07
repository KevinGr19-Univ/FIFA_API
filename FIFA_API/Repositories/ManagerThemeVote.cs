using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerThemeVote : BaseVisibleManager<ThemeVote>, IManagerThemeVote
    {
        public ManagerThemeVote(DbContext context) : base(context) { }

        public async Task<ThemeVote?> GetByIdWithAssocJoueurs(int id)
        {
            return await DbSet.Include(t => t.AssocJoueurs)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ThemeVote?> GetByIdWithJoueurs(int id)
        {
            return await DbSet.Include(t => t.AssocJoueurs)
                .ThenInclude(a => a.Joueur)
                .SingleOrDefaultAsync(t => t.Id == id);
        }
    }
}
