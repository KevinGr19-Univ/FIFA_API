namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class LigneCommande
    {
        public VarianteCouleurProduit VCProduit { get; set; }
        public TailleProduit Taille { get; set; }
        public Commande Commande { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
