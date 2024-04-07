using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerStatusCommande : IRepository<StatusCommande>
    {
        Task<StatusCommande?> Find(int idcommande, CodeStatusCommande code);
        Task<StatusCommande?> GetById(int idcommande, CodeStatusCommande code);
        Task<bool> Exists(int idcommande, CodeStatusCommande code);
    }
}
