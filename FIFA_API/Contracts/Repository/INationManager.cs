using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface INationManager : IRepository<Nation>, IGetById<int, Nation>
    {
    }
}
