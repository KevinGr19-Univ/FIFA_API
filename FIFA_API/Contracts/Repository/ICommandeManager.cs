using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    public interface ICommandeManager : IRepository<Commande>, IGetByIdInt<Commande>
    {
    }
}
