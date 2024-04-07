using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Repositories.Contracts
{
    public interface IManagerDocument : IVisibleRepository<Document>
    {
        Task<Document?> GetByIdWithAll(int id, bool onlyVisible = true);
    }
}
