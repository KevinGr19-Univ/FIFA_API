using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerJoueur : IRepository<Joueur>, IGetEntity<int, Joueur>
    {
        Task<Joueur?> GetByIdWithData(int id);
        Task<Joueur?> GetByIdWithPublications(int id);
    }
}
