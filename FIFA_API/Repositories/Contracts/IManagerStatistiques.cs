using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerStatistiques : IRepository<Statistiques>, IGetEntity<int, Statistiques>
    {
    }
}
