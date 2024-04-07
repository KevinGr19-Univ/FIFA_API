using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public class UnitOfWorkVote : UnitOfWork, IUnitOfWorkVote
    {
        public UnitOfWorkVote(FifaDbContext context) : base(context) { }

        #region Getters
        private IManagerVoteUtilisateur _votes;
        public IManagerVoteUtilisateur Votes
        {
            get
            {
                if (_votes is null) _votes = new ManagerVoteUtilisateur(context);
                return _votes;
            }
        }

        private IManagerJoueur _joueurs;
        public IManagerJoueur Joueurs
        {
            get
            {
                if (_joueurs is null) _joueurs = new ManagerJoueur(context);
                return _joueurs;
            }
        }

        private IManagerThemeVote _themes;
        public IManagerThemeVote Themes
        {
            get
            {
                if(_themes is null) _themes = new ManagerThemeVote(context);
                return _themes;
            }
        }

        private IManagerThemeVoteJoueur _themejoueurs;
        public IManagerThemeVoteJoueur ThemeJoueurs
        {
            get
            {
                if(_themejoueurs is null) _themejoueurs = new ManagerThemeVoteJoueur(context);
                return _themejoueurs;
            }
        }
        #endregion

        public async Task<bool> IsVoteValid(VoteUtilisateur vote)
        {
            return await ThemeJoueurs.IsVoteValid(vote);
        }
    }
}
