using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerThemeVote : IVisibleRepository<ThemeVote>
    {
        Task<ThemeVote?> GetByIdWithAssocJoueurs(int id);
        Task<ThemeVote?> GetByIdWithJoueurs(int id);
    }
}
