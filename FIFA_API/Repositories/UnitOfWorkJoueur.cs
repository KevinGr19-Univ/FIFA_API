using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public class UnitOfWorkJoueur : UnitOfWork, IUnitOfWorkJoueur
    {
        public UnitOfWorkJoueur(FifaDbContext context) : base(context) { }

        #region Getters
        private IManagerJoueur _joueurs;
        public IManagerJoueur Joueurs
        {
            get
            {
                if (_joueurs is null) _joueurs = new ManagerJoueur(context);
                return _joueurs;
            }
        }

        private IManagerPublication _publications;
        public IManagerPublication Publications
        {
            get
            {
                if(_publications is null) _publications = new ManagerPublication(context);
                return _publications;
            }
        }

        private IManagerTrophee _trophees;
        public IManagerTrophee Trophees
        {
            get
            {
                if(_trophees is null) _trophees = new ManagerTrophee(context);
                return _trophees;
            }
        }

        private IManagerFaqJoueur _faqs;
        public IManagerFaqJoueur Faqs
        {
            get
            {
                if(_faqs is null) _faqs = new ManagerFaqJoueur(context);
                return _faqs;
            }
        }
        #endregion
    }
}
