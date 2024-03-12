namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class CategorieProduit
    {
        public string Nom { get; set; }

        public CategorieProduit? Parent { get; set; }
        public ICollection<CategorieProduit> SousCategories { get; set; }
        public ICollection<Produit> Produits { get; set; }
    }
}
