using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_utilisateur_utl")]
	public partial class Utilisateur
	{
        [Key]
        [Column("utl_id")]
        public int Id { get; set; }

        [Column("utl_idlangue")]
        public int IdLangue { get; set; }

        [Column("utl_idpays")]
        public int IdPays { get; set; }

        [Column("utl_idpaysfavori")]
        public int IdPaysFavori { get; set; }

        [Column("utl_stripeid")]
        [StringLength(100)]
        public string StripeId { get; set; }

        [Column("utl_phone")]
        [StringLength(50)]
        public string Phone { get; set; }

        [Column("utl_prenom")]
        [StringLength(100)]
        public string Prenom { get; set; }

        [Column("utl_mail")]
        [StringLength(100)]
        public string Mail { get; set; }

        [Column("utl_surnom")]
        [StringLength(100)]
        public string Surnom { get; set; }

        [Column("utl_datenaissance")]
        public DateTime DateNaissance { get; set; }

        [Column("utl_motpasse")]
        [StringLength(200)]
        public string MotPasse { get; set; }

        [Column("utl_role")]
        [StringLength(50)]
        public string Role { get; set; }

        [Column("utl_lastconnection")]
        public DateTime LastConnection { get; set; }

        [Column("utl_emailverified")]
        public DateTime EmailVerified { get; set; }

        [Column("utl_doublefa")]
        public bool DoubleFA { get; set; }
    }
}