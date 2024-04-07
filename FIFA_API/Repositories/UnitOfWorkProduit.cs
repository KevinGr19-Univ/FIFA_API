using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public sealed class UnitOfWorkProduit : UnitOfWork, IUnitOfWorkProduit
    {
        public UnitOfWorkProduit(DbContext context) : base(context) { }

        private IManagerCategorieProduit _categories;
        public IManagerCategorieProduit Categories
        {
            get
            {
                if(_categories is null) _categories = new ManagerCategorieProduit(context);
                return _categories;
            }
        }

        private IManagerProduit _produits;
        public IManagerProduit Produits
        {
            get
            {
                if(_produits is null) _produits = new ManagerProduit(context);
                return _produits;
            }
        }

        private IManagerVarianteCouleurProduit _variantes;
        public IManagerVarianteCouleurProduit Variantes
        {
            get
            {
                if(_variantes is null) _variantes = new ManagerVarianteCouleurProduit(context);
                return _variantes;
            }
        }

        private IManagerCouleur _couleurs;
        public IManagerCouleur Couleurs
        {
            get
            {
                if(_couleurs is null) _couleurs = new ManagerCouleur(context);
                return _couleurs;
            }
        }

        private IManagerTailleProduit _tailles;
        public IManagerTailleProduit Tailles
        {
            get
            {
                if(_tailles is null) _tailles = new ManagerTailleProduit(context);
                return _tailles;
            }
        }

        private IManagerCompetition _competitions;
        public IManagerCompetition Competitions
        {
            get
            {
                if(_competitions is null) _competitions = new ManagerCompetition(context);
                return _competitions;
            }
        }

        private IManagerGenre _genres;
        public IManagerGenre Genres
        {
            get
            {
                if(_genres is null) _genres = new ManagerGenre(context);
                return _genres;
            }
        }

        private IManagerNation _nations;
        public IManagerNation Nations
        {
            get
            {
                if(_nations is null) _nations = new ManagerNation(context);
                return _nations;
            }
        }
    }
}
