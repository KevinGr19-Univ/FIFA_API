using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts
{
    public interface ILogin2FAService
    {
        Task<string> Send2FACodeAsync(Utilisateur user);

        Task<Utilisateur?> AuthenticateAsync(string token, string code);
    }
}
