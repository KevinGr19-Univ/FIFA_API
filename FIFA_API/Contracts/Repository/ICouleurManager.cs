using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface ICouleurManager : IRepository<Couleur>, IGetByIdInt<Couleur>
    {
    }
}
