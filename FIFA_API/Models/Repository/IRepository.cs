using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Models.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<ActionResult<IEnumerable<T>>> GetAllAsync();
        Task<ActionResult<T?>> GetByIdAsync(params object[] id);
        Task AddAsync(T elementToAdd);
        Task UpdateAsync(T elementToUpdate, T sourceElement);
        Task DeleteAsync(T elementToDelete);
    }
}
