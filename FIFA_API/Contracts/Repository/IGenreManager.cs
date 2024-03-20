using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface IGenreManager : IRepository<Genre>, IGetById<int, Genre>
    {
    }
}
