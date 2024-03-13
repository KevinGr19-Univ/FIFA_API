using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    //Ajouter les roles
    public enum RoleUtilisateur
    {
        Utilisateur = 0,
        DirecteurDuServiceVentes = 1,
        MembreDuServiceExpedition = 2,
        MembreDuServiceCommande = 3
    }

	[Table("t_e_utilisateur_utl")]
    public class Utilisateur
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("utl_id")]
        public int Id { get; set; }

        [Column("lan_id")]
        public int IdLangue { get; set; }
        [ForeignKey(nameof(IdLangue))]
        public Langue Langue { get; set; }

        [Column("pys_idpays")]
        public int IdPays { get; set; }
        [ForeignKey(nameof(IdPays))]
        public Pays Pays { get; set; }

        [Column("utl_idpaysfavori")]
        public int IdPaysFavori { get; set; }
        [ForeignKey(nameof(IdPaysFavori))]
        public Pays PaysFavori { get; set; }

        [StringLength(100, ErrorMessage = "L'id de stripe ne doit pas d�passer les 100 caract�res")]
        [Column("utl_stripeid")]
        public string StripeId { get; set; }

        [RegularExpression(ModelUtils.REGEX_TELEPHONE, ErrorMessage = "Le num�ro de t�l�phone doit contenir 10 chiffres, dont le 1er doit �tre un 0")]
        [Column("utl_telephone")]
        public string Telephone { get; set; }

        [StringLength(100, ErrorMessage = "Le pr�nom ne doit pas d�passer les 100 caract�res")]
        [Column("utl_prenom")]
        public string Prenom { get; set; }

        [EmailAddress(ErrorMessage = "L'addresse Email doit convenir aux normes des adresses email")]
        [Column("utl_mail")]
        public string Mail { get; set; }

        [StringLength(100, ErrorMessage = "Le surnom ne doit pas d�passer les 100 caract�res")]
        [Column("utl_surnom")]
        public string Surnom { get; set; }

		[Column("utl_datenaissance", TypeName = "date")]
        public DateTime DateNaissance { get; set; }

        [RegularExpression(ModelUtils.REGEX_PASSWORD, ErrorMessage = "Le mot de passe doit contenir entre 12 et 20 caract�res avec au moins 1 lettre majuscule, 1 chiffre et 1 caract�re sp�cial")]
        [Column("utl_motdepasse")]
        public string MotDePasse { get; set; }

        [Column("utl_role")]
        public RoleUtilisateur Role { get; set; }

		[Column("utl_derniereconnexion")]
        public DateTime DerniereConnexion { get; set; }

		[Column("utl_dateverificationemail")]
        public DateTime DateVerificationEmail { get; set; }

		[Column("utl_doubleauthentification")]
        public bool DoubleAuthentification { get; set; }

    }
}
