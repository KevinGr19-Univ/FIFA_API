using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.LE_CODE_FIRST____
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


        [Column("utl_stripeid")]
        public string StripeId { get; set; }

		[Column("utl_telephone")]
        public string Telephone { get; set; }

		[Column("utl_prenom")]
        public string Prenom { get; set; }

		[Column("utl_mail")]
        public string Mail { get; set; }

		[Column("utl_surnom")]
        public string Surnom { get; set; }

		[Column("utl_datenaissance")]
        public DateTime DateNaissance { get; set; }

		[Column("utl_motdepasse")]
        public string MotDePasse { get; set; }

        public RoleUtilisateur Role { get; set; }

		[Column("utl_derniereconnexion")]
        public DateTime DerniereConnexion { get; set; }

		[Column("utl_dateverificationemail")]
        public DateTime DateVerificationEmail { get; set; }

		[Column("utl_doubleauthentification")]
        public bool DoubleAuthentification { get; set; }

    }
}
