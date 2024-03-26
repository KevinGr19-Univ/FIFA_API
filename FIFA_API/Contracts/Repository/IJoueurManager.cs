using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;

namespace FIFA_API.Contracts.Repository
{
    public interface IJoueurManager : IRepository<Joueur>, IGetById<int, Joueur>
    {
    }
}
