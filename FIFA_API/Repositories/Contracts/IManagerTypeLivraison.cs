using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerTypeLivraison : IRepository<TypeLivraison>, IGetEntity<int, TypeLivraison>
    {
    }
}
