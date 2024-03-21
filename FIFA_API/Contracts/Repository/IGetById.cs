namespace FIFA_API.Contracts.Repository
{
    /// <summary>
    /// Contract used to get an entity from its typed identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity key.</typeparam>
    /// <typeparam name="TEntity">The entity to manage.</typeparam>
    public interface IGetById<TKey,TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets an entity from its <see href="TKey"/> identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The matching entity if found, <see langword="null"/> otherwise.</returns>
        Task<TEntity?> GetByIdAsync(TKey id);
    }
}
