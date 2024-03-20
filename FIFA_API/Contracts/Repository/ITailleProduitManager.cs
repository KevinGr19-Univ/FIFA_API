using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface ITailleProduitManager : IRepository<TailleProduit>, IGetById<int, TailleProduit>
    {
    }
}
