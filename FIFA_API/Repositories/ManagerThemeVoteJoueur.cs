using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Repositories
{
    public sealed class ManagerThemeVoteJoueur : BaseManager<ThemeVoteJoueur>, IManagerThemeVoteJoueur
    {
        public ManagerThemeVoteJoueur(FifaDbContext context) : base(context) { }

        public async Task<ThemeVoteJoueur?> GetById(int idtheme, int idjoueur)
        {
            return await DbSet.FindAsync(idtheme, idjoueur);
        }

        public async Task<bool> IsVoteValid(VoteUtilisateur vote)
        {
            return await DbSet.Where(t => t.IdTheme == vote.IdTheme
                && (t.IdJoueur == vote.IdJoueur1
                    || t.IdJoueur == vote.IdJoueur2
                    || t.IdJoueur == vote.IdJoueur3))
                .CountAsync() == 3;
        }
    }
}
