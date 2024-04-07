namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWorkJoueur : IUnitOfWork
    {
        IManagerJoueur Joueurs { get; }
        IManagerPublication Publications { get; }
        IManagerTrophee Trophees { get; }
        IManagerFaqJoueur Faqs { get; }
    }
}
