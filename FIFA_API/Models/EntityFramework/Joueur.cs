using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_joueur_jou")]
    public partial class Joueur
	{
        public enum Pied { Droitier = 0, Gaucher = 1, Ambidextre = 2}

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
        [StringLength(100, ErrorMessage = "Le nom ne doit pas dépasser 100 caractères")]
        public string NomJoueur { get; set; } = null!;

        [Column("jou_prenom")]
        [StringLength(100, ErrorMessage = "Le prénom ne doit pas dépasser 100 caractères")]
        public string PrenomJoueur { get; set; } = null!;

        [Column("jou_datenaissance")]
        public DateTime? DateNaissanceJoueur { get; set; }

        [Column("jou_lieunaissance")]
        [StringLength(100, ErrorMessage = "Le lieu de naissance ne doit pas dépasser 100 caractères")]
        public string LieuNaissanceJoueur { get; set; } = null!;

        [Column("jou_pied")]
        [StringLength(20, ErrorMessage = "Le pied maître du joueur ne doit pas dépasser 20 caractères")]
        public Pied PiedJoueur { get; set; }
        [Column("jou_poids")]
        public int PoidsJoueur { get; set; }

        [Column("jou_taille")]
        public int TailleJoueur { get; set; }

        [Column("jou_poste")]
        [StringLength(50, ErrorMessage = "Le poste du joueur ne doit pas dépasser 50 caractères")]
        public string PosteJoueur { get; set; } = null!;

        [Column("jou_biographie")]
        [StringLength(500, ErrorMessage = "La biographie ne doit pas dépasser 500 caractères")]
        public string Biographie { get; set; } = null!;

        [Column("jou_urlphoto")]
        [StringLength(500, ErrorMessage = "L'Url de la photo ne doit pas dépasser 500 caractères")]
        public string UrlPhotoJoueur { get; set; } = null!;
    }
}