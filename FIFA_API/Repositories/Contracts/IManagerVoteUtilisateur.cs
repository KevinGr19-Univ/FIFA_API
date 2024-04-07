using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerVoteUtilisateur : IRepository<VoteUtilisateur>
    {
        Task<VoteUtilisateur?> GetById(int idtheme, int iduser);
        Task<bool> Exists(int idtheme, int iduser);

        Task<IEnumerable<VoteUtilisateur>> GetUserVotes(int iduser);
    }
}
