using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts.Repository
{
    /// <summary>
    /// Contract used to manage instances of <see cref="Produit"/>.
    /// </summary>
    public interface IProduitManager : IRepository<Produit>, IGetById<int, Produit>
    {
        /// <summary>
        /// Gets all instances of <see cref="Produit"/> matching the given query and filters.
        /// </summary>
        /// <param name="query">The query to use.</param>
        /// <returns>All matching instances of <see cref="Produit"/></returns>
        Task<IEnumerable<Produit>> SearchProductsAsync(string query);
    }
}
