using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    public abstract partial class Publication
    {
        [NotMapped]
        public string Type => GetType().Name.ToLower();

        /// <summary>
        /// Trie une liste de publication par type (<seealso cref="Type"/>).
        /// </summary>
        /// <param name="publications">Les publications à trier.</param>
        /// <returns>Un dictionnaire des ids de publication par type.</returns>
        public static Dictionary<string, List<int>> SortPublicationsIds(IEnumerable<Publication> publications)
        {
            Dictionary<string, List<int>> ids = new();
            foreach(var pub in publications)
            {
                if (!ids.ContainsKey(pub.Type)) ids.Add(pub.Type, new());
                ids[pub.Type].Add(pub.Id);
            }

            return ids;
        }
    }
}
