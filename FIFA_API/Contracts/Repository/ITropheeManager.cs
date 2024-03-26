using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface ITropheeManager : IRepository<Trophee>, IGetById<int, Trophee>
    {
    }
}
