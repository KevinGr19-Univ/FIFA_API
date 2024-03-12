namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class VarianteCouleurProduit
    {
        public Produit Produit { get; set; }
        public Couleur Couleur { get; set; }
        public decimal Prix { get; set; }

        public ICollection<string> ImageUrls { get; set; }
    }
}
