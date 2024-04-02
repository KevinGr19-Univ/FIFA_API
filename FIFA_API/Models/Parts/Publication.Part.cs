using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    public abstract partial class Publication
    {
        [NotMapped]
        public string Type => GetType().Name.ToLower();

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
