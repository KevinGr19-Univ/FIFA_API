using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_joueur_jou")]
    public partial class Joueur
	{
        [Key]
        [Column("jou_id")]
        public int Id { get; set; }

        //Foreign Key
        [Column("jou_idclub")]
        public int IdClub { get; set; }

        //Foreign Key
        [Column("jou_idpays")]
        public int IdPays { get; set; }

        [Column("jou_nom")]
        [StringLength(100)]
        public string NomJoueur { get; set; } = null!;

        [Column("jou_prenom")]
        [StringLength(100)]
        public string PrenomJoueur { get; set; } = null!;

        [Column("jou_datenaissance")]
        public DateTime? DateNaissanceJoueur { get; set; }

        [Column("jou_lieunaissance")]
        [StringLength(100)]
        public string LieuNaissanceJoueur { get; set; } = null!;

        [Column("jou_pied")]
        [StringLength(20)]
        public string PiedJoueur { get; set; } = null!;

        [Column("jou_poids")]
        public int PoidsJoueur { get; set; }

        [Column("jou_taille")]
        public int TailleJoueur { get; set; }

        [Column("jou_poste")]
        [StringLength(50)]
        public string Postejoueur { get; set; } = null!;

        [Column("jou_biographie")]
        [StringLength(500)]
        public string Biographie { get; set; } = null!;

        [Column("jou_photo")]
        [StringLength(500)]
        public string PhotoJoueur { get; set; } = null!;
    }
}