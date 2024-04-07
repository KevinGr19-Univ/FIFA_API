using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerPhoto : IRepository<Photo>, IGetEntity<int, Photo>
    {
    }
}
