using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerAuthPasswordReset : IRepository<AuthPasswordReset>, IGetEntity<string, AuthPasswordReset>
    {
        Task<AuthPasswordReset?> GetByCode(string code);
    }
}
