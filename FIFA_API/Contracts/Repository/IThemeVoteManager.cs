using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface IThemeVoteManager : IRepository<ThemeVote>, IGetById<int, ThemeVote>
    {
    }
}
