using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_joueur_jou")]
    public partial class Joueur
    {
        public const int MAX_NOM_LENGTH = 100;
        public const int MAX_BIO_LENGTH = 500;

        public Joueur()
        {
            FaqJoueurs = new HashSet<FaqJoueur>();
            Trophees = new HashSet<Trophee>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("jou_id")]
        public int Id { get; set; }

        [Column("jou_nom")]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 100 caractères.")]
        public string Nom { get; set; } = null!;

        [Column("jou_prenom")]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le prénom ne doit pas dépasser 100 caractères.")]
        public string Prenom { get; set; } = null!;

        [Column("jou_datenaissance", TypeName = "date")]
        public DateTime? DateNaissance { get; set; }

        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le lieu de naissance ne doit pas dépasser 100 caractères.")]
        [Column("jou_lieunaissance")]
        public string LieuNaissance { get; set; } = null!;


        [Column("jou_poids")]
        [Range(0, int.MaxValue, ErrorMessage = "Le poids du joueur doit être positif.")]
        public int Poids { get; set; }

        [Column("jou_taille")]
        [Range(0, int.MaxValue, ErrorMessage = "La taille du joueur doit être positive.")]
        public int Taille { get; set; }


        [Column("jou_biographie")]
        [StringLength(MAX_BIO_LENGTH, ErrorMessage = "La biographie ne doit pas dépasser 500 caractères.")]
        public string Biographie { get; set; } = null!;

        //Photo
        [Column("pht_id")]
        public int? IdPhoto { get; set; }

        [ForeignKey(nameof(IdPhoto))]
        public Photo? Photo { get; set; } = null!;

        //Statistiques
        [InverseProperty(nameof(Statistiques.Joueur))]
        public Statistiques? Stats { get; set; }

        //Club
        [Column("clb_id")]
        public int IdClub {  get; set; }

        [ForeignKey(nameof(IdClub))]
        public Club Club { get; set; }

        //Pays
        [Column("pys_id")]
        public int IdPays { get; set; }

        [ForeignKey(nameof(IdPays))]
        public Pays Pays { get; set; }

        // Enums
        [Column("pjo_id")]
        public int IdPied { get; set; }

        [ForeignKey(nameof(IdPied))]
        public PiedJoueur Pied { get; set; }

        [Column("poj_id")]
        public int IdPoste { get; set; }

        [ForeignKey(nameof(IdPoste))]
        public PosteJoueur Poste { get; set; }


        [InverseProperty(nameof(FaqJoueur.Joueur))]
        public virtual ICollection<FaqJoueur> FaqJoueurs { get; set; }

        [ManyToMany(nameof(Trophee.Joueurs))]
        public virtual ICollection<Trophee> Trophees { get; set; }

        [ManyToMany("_joueurs")]
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
