using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerClub : IRepository<Club>, IGetEntity<int, Club>
    {
    }
}
