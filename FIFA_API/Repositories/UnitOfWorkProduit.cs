using FIFA_API.Models.Contracts;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using FIFA_API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FIFA_API.Repositories
{
    public sealed class UnitOfWorkProduit : UnitOfWork, IUnitOfWorkProduit
    {
        public UnitOfWorkProduit(FifaDbContext context) : base(context) { }

        #region Getters
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

        private IManagerStockProduit _stocks;
        public IManagerStockProduit Stocks
        {
            get
            {
                if (_stocks is null) _stocks = new ManagerStockProduit(context);
                return _stocks;
            }
        }
        #endregion

        public async Task<IEnumerable<SearchProductItem>> SearchProduits(
            string? q,
            int[] categories,
            int[] competitions,
            int[] genres,
            int[] nations,
            int[] couleurs,
            int[] tailles,
            bool? desc,
            int page,
            int amount)
        {
            return await Produits.SearchProduits(async initQuery =>
            {
                var query = initQuery
                .Where(p => p.Visible)
                .Include(p => p.Variantes)
                .ThenInclude(v => v.Couleur)
                .Include(p => p.Tailles)
                .Select(p => new
                {
                    Produit = p,
                    Variantes = p.Variantes.Where(c => c.Visible && c.Couleur.Visible),
                    Tailles = p.Tailles.Where(t => t.Visible).Select(t => t.Id)
                })
                .Where(p => p.Variantes.Count() > 0 && p.Tailles.Count() > 0);

                categories = await Categories.FilterVisibleIds(categories);
                competitions = await Competitions.FilterVisibleIds(competitions);
                genres = await Genres.FilterVisibleIds(genres);
                nations = await Nations.FilterVisibleIds(nations);
                couleurs = await Couleurs.FilterVisibleIds(couleurs);
                tailles = await Tailles.FilterVisibleIds(tailles);

                if (categories.Length > 0)
                    query = query.Where(p => categories.Contains(p.Produit.IdCategorieProduit));

                if (competitions.Length > 0)
                    query = query.Where(p => p.Produit.IdCompetition != null && competitions.Contains((int)p.Produit.IdCompetition));

                if (genres.Length > 0)
                    query = query.Where(p => p.Produit.IdGenre != null && genres.Contains((int)p.Produit.IdGenre));

                if (nations.Length > 0)
                    query = query.Where(p => p.Produit.IdNation != null && nations.Contains((int)p.Produit.IdNation));

                if (couleurs.Length > 0)
                    query = query.Where(p => p.Variantes.Any(c => couleurs.Contains(c.IdCouleur)));

                if (tailles.Length > 0)
                    query = query.Where(p => p.Tailles.Any(t => tailles.Contains(t)));

                if (q is not null)
                    query = query.Where(p => p.Produit.Titre.ToLower().Contains(q.ToLower()));

                if (desc == false)
                    query = query.OrderBy(p => p.Produit.Variantes.Min(v => v.Prix));

                else if (desc == true)
                    query = query.OrderByDescending(p => p.Produit.Variantes.Min(v => v.Prix));

                query = query.Paginate(page, amount);

                return query.Select(p =>
                    new {
                        p.Produit,
                        Couleurs = p.Variantes.Select(v => v.IdCouleur),
                        p.Tailles,
                        MinVariante = p.Produit.Variantes.OrderBy(v => v.Prix).First()
                    }
                ).Select(p =>
                    new SearchProductItem
                    {
                        Id = p.Produit.Id,
                        Titre = p.Produit.Titre,
                        Prix = p.MinVariante.Prix,
                        Couleurs = p.Couleurs.ToArray(),
                        Tailles = p.Tailles.ToArray(),
                        ImageUrl = p.MinVariante.ImageUrls[0]
                    }
                );
            });
        }
    }
}
