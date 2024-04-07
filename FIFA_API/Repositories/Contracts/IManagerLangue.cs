using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerLangue : IRepository<Langue>, IGetEntity<int, Langue>
    {
    }
}
