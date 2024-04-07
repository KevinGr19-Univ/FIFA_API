using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWorkVote : IUnitOfWork
    {
        IManagerVoteUtilisateur Votes { get; }
        IManagerJoueur Joueurs { get; }
        IManagerThemeVote Themes { get; }
        IManagerThemeVoteJoueur ThemeJoueurs { get; }

        Task<bool> IsVoteValid(VoteUtilisateur vote);
    }
}
