namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWorkUserServices : IUnitOfWork
    {
        IManagerUtilisateur Utilisateurs { get; }
        IManagerAuth2FALogin Login2FAs { get; }
        IManagerAuthEmailVerif EmailVerifs { get; }
        IManagerAuthPasswordReset PasswordResets { get; }
    }
}
