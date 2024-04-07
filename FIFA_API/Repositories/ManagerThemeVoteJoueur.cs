using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerThemeVoteJoueur : BaseManager<ThemeVoteJoueur>, IManagerThemeVoteJoueur
    {
        public ManagerThemeVoteJoueur(DbContext context) : base(context) { }

        public async Task<ThemeVoteJoueur?> GetById(int idtheme, int idjoueur)
        {
            return await DbSet.FindAsync(idtheme, idjoueur);
        }
    }
}
