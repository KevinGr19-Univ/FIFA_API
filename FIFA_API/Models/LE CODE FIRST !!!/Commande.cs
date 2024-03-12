namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class Commande
    {
        public TypeLivraison TypeLivraison { get; set; }
        public Utilisateur Utilisateur { get; set; }
        public Adresse AdresseLivraison { get; set; }
        public Adresse AdresseFacturation { get; set; }
        public decimal PrixLivraison { get; set; }
        public DateTime DateExpedition { get; set; }
        public DateTime? DateCommande { get; set; }
        public DateTime? DateLivraison { get; set; }
        public string UrlFacture { get; set; }

        public ICollection<LigneCommande> Lignes { get; set; }
        public ICollection<StatusCommande> Status { get; set; }
    }
}
