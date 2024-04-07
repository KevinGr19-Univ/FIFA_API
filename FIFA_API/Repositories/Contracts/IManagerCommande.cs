using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerCommande : IRepository<Commande>, IGetEntity<int, Commande>
    {
        Task<Commande?> GetByIdWithStatus(int id);
        Task<Commande?> GetByIdWithLignes(int id);
        Task<Commande?> GetByIdWithAll(int id);

        Task<IEnumerable<ApercuCommande>> SearchCommandes(int? iduser, int[] typesLivraison, bool? desc, int page, int amount);
    }
}
