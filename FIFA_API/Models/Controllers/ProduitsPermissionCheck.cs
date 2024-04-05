namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Informations sur les permissions pour les produits.
    /// </summary>
    public class ProduitsPermissionCheck
    {
        /// <summary>
        /// Permission d'ajouter des produits et autres.
        /// </summary>
        public bool Add { get; set; }

        /// <summary>
        /// Permission d'éditer des produits et autres.
        /// </summary>
        public bool Edit { get; set; }

        /// <summary>
        /// Permission de supprimer des produits et autres.
        /// </summary>
        public bool Delete { get; set; }
    }
}
