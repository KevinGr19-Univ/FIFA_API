using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts
{
    public interface IEmailVerificator
    {
        Task SendVerificationAsync(Utilisateur user);

        Task<bool> Verify(Utilisateur user, string code);
    }
}
