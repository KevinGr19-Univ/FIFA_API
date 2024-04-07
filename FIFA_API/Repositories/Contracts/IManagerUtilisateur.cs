using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerUtilisateur : IRepository<Utilisateur>, IGetEntity<int, Utilisateur>
    {
        Task<Utilisateur?> GetByEmail(string mail);
        Task<Utilisateur?> GetBy2FAToken(string token2fa);
        Task<bool> IsEmailTaken(string mail);
    }
}
