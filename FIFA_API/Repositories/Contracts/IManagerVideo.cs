using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerVideo : IRepository<Video>, IGetEntity<int, Video>
    {
    }
}
