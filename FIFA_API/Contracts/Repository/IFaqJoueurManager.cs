using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface IFaqJoueurManager : IRepository<FaqJoueur>, IGetById<int, FaqJoueur>
    {
    }
}
