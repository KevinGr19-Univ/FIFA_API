using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_utilisateur_utl")]
	public partial class Utilisateur
	{
        [Key]
        [Column("utl_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("utl_idlangue")]
        public int IdLangue { get; set; }

        [Column("utl_idpays")]
        public int IdPays { get; set; }

        [Column("utl_idpaysfavori")]
        public int IdPaysFavori { get; set; }

        [Column("utl_stripeid")]
        [StringLength(100, ErrorMessage = "L'id stripe ne doit pas d�passer 100 caract�res")]
        public string StripeId { get; set; }

        [Column("utl_phone")]
        [StringLength(50, ErrorMessage = "Le phone ne doit pas d�passer 50 caract�res")]
        public string Phone { get; set; }

        [Column("utl_prenom")]
        [StringLength(100, ErrorMessage = "Le pr�nom ne doit pas d�passer 100 caract�res")]
        public string Prenom { get; set; }

        [Column("utl_mail")]
        [StringLength(100, ErrorMessage = "Le mail ne doit pas d�passer 100 caract�res")]
        public string Mail { get; set; }

        [Column("utl_surnom")]
        [StringLength(100, ErrorMessage = "Le surnom ne doit pas d�passer 100 caract�res")]
        public string Surnom { get; set; }

        [Column("utl_datenaissance", TypeName = "date")]
        public DateTime DateNaissance { get; set; }

        [Column("utl_motpasse")]
        [StringLength(200, ErrorMessage = "Le mot de passe ne doit pas d�passer 200 caract�res")]
        public string MotPasse { get; set; }

        [Column("utl_role")]
        [StringLength(50, ErrorMessage = "Le role ne doit pas d�passer 50 caract�res")]
        public string Role { get; set; }

        [Column("utl_lastconnection")]
        public DateTime LastConnection { get; set; }

        [Column("utl_emailverified")]
        public DateTime EmailVerified { get; set; }

        [Column("utl_doublefa")]
        public bool DoubleFA { get; set; }
    }
}