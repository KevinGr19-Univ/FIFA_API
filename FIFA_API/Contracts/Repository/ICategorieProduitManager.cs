using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface ICategorieProduitManager : IRepository<CategorieProduit>, IGetByIdInt<CategorieProduit>
    {
    }
}
