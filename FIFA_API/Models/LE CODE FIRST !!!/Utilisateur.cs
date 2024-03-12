namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class Utilisateur
    {
        //Ajouter les roles
        public enum RoleUtilisateur { Utilisateur = 0, DirecteurDuServiceVentes = 1, MembreDuServiceExpedition = 2, MembreDuServiceCommande = 3}
        public string StripeId { get; set; }
        public string Telephone { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Surnom { get; set; }
        public DateTime DateNaissance { get; set; }
        public string MotDePasse { get; set; }
        public RoleUtilisateur Role { get; set; }
        public DateTime DerniereConnexion { get; set; }
        public DateTime DateVerificationEmail { get; set; }
        public bool DoubleAuthentification { get; set; }
    }
}
