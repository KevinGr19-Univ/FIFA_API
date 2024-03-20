using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface IPaysManager : IRepository<Pays>, IGetById<int, Pays>
    {
    }
}
