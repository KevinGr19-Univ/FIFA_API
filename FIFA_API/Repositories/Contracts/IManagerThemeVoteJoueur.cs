using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerThemeVoteJoueur : IRepository<ThemeVoteJoueur>
    {
        Task<ThemeVoteJoueur?> GetById(int idtheme, int idjoueur);
        Task<bool> IsVoteValid(VoteUtilisateur vote);
    }
}
