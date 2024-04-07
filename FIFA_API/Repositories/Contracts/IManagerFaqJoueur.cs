using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerFaqJoueur : IRepository<FaqJoueur>, IGetEntity<int, FaqJoueur>
    {
    }
}
