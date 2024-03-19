namespace FIFA_API.Contracts.Repository
{
    /// <summary>
    /// Contract used to get an entity from its <see cref="int"/> identifier.
    /// </summary>
    /// <typeparam name="T">The entity to manage.</typeparam>
    public interface IGetByIdInt<T> where T : class
    {
        /// <summary>
        /// Gets an entity from its <see cref="int"/> identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The matching entity if found, <see langword="null"/> otherwise.</returns>
        Task<T?> GetByIdAsync(int id);
    }
}
