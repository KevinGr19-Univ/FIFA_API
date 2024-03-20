using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface ICompetitionManager : IRepository<Competition>, IGetById<int, Competition>
    {
    }
}
