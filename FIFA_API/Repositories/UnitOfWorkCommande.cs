using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Repositories
{
    public class UnitOfWorkCommande : UnitOfWork, IUnitOfWorkCommande
    {
        public UnitOfWorkCommande(FifaDbContext context) : base(context) { }

        #region Getters
        private IManagerCommande _commandes;
        public IManagerCommande Commandes
        {
            get
            {
                if (_commandes is null) _commandes = new ManagerCommande(context);
                return _commandes;
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

        private IManagerCouleur _couleurs;
        public IManagerCouleur Couleurs
        {
            get
            {
                if(_couleurs is null) _couleurs = new ManagerCouleur(context);
                return _couleurs;
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

        private IManagerTailleProduit _tailles;
        public IManagerTailleProduit Tailles
        {
            get
            {
                if(_tailles is null) _tailles = new ManagerTailleProduit(context);
                return _tailles;
            }
        }

        private IManagerStockProduit _stocks;
        public IManagerStockProduit Stocks
        {
            get
            {
                if(_stocks is null) _stocks = new ManagerStockProduit(context);
                return _stocks;
            }
        }

        private IManagerTypeLivraison _typelivraisons;
        public IManagerTypeLivraison TypeLivraisons
        {
            get
            {
                if(_typelivraisons is null) _typelivraisons = new ManagerTypeLivraison(context);
                return _typelivraisons;
            }
        }

        private IManagerUtilisateur _utilisateurs;
        public IManagerUtilisateur Utilisateurs
        {
            get
            {
                if(_utilisateurs is null) _utilisateurs = new ManagerUtilisateur(context);
                return _utilisateurs;
            }
        }
        #endregion

        public async Task<CommandeDetails> GetDetails(Commande commande)
        {
            var details = new CommandeDetails()
            {
                Commande = commande,
                Produits = new(),
                Couleurs = new(),
                Tailles = new()
            };

            foreach (var ligne in commande.Lignes)
            {
                var variante = await Variantes.GetByIdWithData(ligne.IdVCProduit);
                details.Produits.Add(variante.IdProduit, variante.Produit.Titre);
                details.Couleurs.Add(variante.IdCouleur, variante.Couleur.Nom);

                var taille = await Tailles.GetById(ligne.IdTaille);
                details.Tailles.Add(taille.Id, taille.Nom);
            }

            return details;
        }
    }
}
