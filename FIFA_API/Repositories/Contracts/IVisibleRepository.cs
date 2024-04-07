using FIFA_API.Models.Contracts;

namespace FIFA_API.Repositories.Contracts
{
    public interface IVisibleRepository<TEntity> : IRepository<TEntity> where TEntity : class, IVisible
    {
        Task<IEnumerable<TEntity>> GetAll(bool onlyVisible = true);
        Task<TEntity?> GetById(int id, bool onlyVisible = true);
        Task<bool> Exists(int id);
    }
}
