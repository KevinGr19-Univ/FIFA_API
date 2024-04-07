namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWorkProduit : IUnitOfWork
    {
        IManagerCategorieProduit Categories { get; }
        IManagerProduit Produits { get; }
        IManagerVarianteCouleurProduit Variantes { get; }
        IManagerCouleur Couleurs { get; }
        IManagerTailleProduit Tailles { get; }

        IManagerCompetition Competitions { get; }
        IManagerGenre Genres { get; }
        IManagerNation Nations { get; }
    }
}
