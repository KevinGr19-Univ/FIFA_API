using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Contracts.Repository
{
    /// <summary>
    /// Contract used to manage a set of entities in a database.
    /// </summary>
    /// <typeparam name="T">The entity type to manage.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all instances of the managed entity.
        /// </summary>
        /// <returns>All instances of the managed entity.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Adds the entity to the set.
        /// </summary>
        /// <param name="elementToAdd">The entity to add.</param>
        Task AddAsync(T elementToAdd);

        /// <summary>
        /// Updates an entity from another's properties and saves the changes.
        /// </summary>
        /// <param name="elementToUpdate">The element to update.</param>
        /// <param name="sourceElement">The element to get the new data from.</param>
        Task UpdateAsync(T elementToUpdate, T sourceElement);

        /// <summary>
        /// Deletes an entity from the set.
        /// </summary>
        /// <param name="elementToDelete">The element to delete.</param>
        Task DeleteAsync(T elementToDelete);
    }
}
