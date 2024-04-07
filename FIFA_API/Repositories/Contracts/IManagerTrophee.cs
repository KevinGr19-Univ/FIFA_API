using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerTrophee : IRepository<Trophee>, IGetEntity<int, Trophee>
    {
        Task<Trophee?> GetByIdWithJoueurs(int id);
    }
}
