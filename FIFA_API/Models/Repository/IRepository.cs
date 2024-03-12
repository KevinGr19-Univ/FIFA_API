using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Models.Repository
{
    public interface IRepository<T>
    {
        Task<ActionResult<IEnumerable<T>>> GetAllAsync();
        Task AddAsync(T elementToAdd);
        Task UpdateAsync(T elementToUpdate, T sourceElement);
        Task DeleteAsync(T elementToDelete);
    }
}
