namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class Competition
    {
        public string Nom { get; set; }

        public ICollection<Produit> Produits { get; set; }
    }
}
