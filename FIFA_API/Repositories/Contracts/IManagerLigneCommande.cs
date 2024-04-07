using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerLigneCommande : IRepository<LigneCommande>, IGetEntity<int, LigneCommande>
    {
    }
}
