namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public enum CodeStatusCommande
    {
        Preparation = 0,
        Validation = 1,
        Expedition = 2,
        Livre = 3,
        Annule = 4,
        Refuse = 5,
        RefusAccepte = 6,
    }

    public class StatusCommande
    {
        public Commande Commande { get; set; }
        public CodeStatusCommande Code { get; set; }
        public DateTime Date { get; set; }
        public string Commentaire { get; set; }
    }
}
