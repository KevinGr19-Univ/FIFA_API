using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
    public enum PiedJoueur
    {
        Gaucher = 0,
        Droitier = 1,
        Ambidextre = 2
    }

    public enum PosteJoueur
    {
        // TODO: A remplir
        Attaquant = 0,
        Defenseur = 1,
        Milieu = 2,
        Gardien = 3
    }

    [Table("t_e_joueur_jou")]
    public partial class Joueur
    {
        public const int MAX_NOM_LENGTH = 100;
        public const int MAX_BIO_LENGTH = 500;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("jou_id")]
        public int Id { get; set; }

        [Column("jou_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 100 caractères.")]
        public string Nom { get; set; } = null!;

        [Column("jou_prenom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le prénom ne doit pas dépasser 100 caractères.")]
        public string Prenom { get; set; } = null!;

        [Column("jou_datenaissance", TypeName = "date")]
        public DateTime? DateNaissance { get; set; }

        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le lieu de naissance ne doit pas dépasser 100 caractères.")]
        [Column("jou_lieunaissance"), Required]
        public string LieuNaissance { get; set; } = null!;


        [Column("jou_poids"), Required]
        [Range(0, int.MaxValue, ErrorMessage = "Le poids du joueur doit être positif.")]
        public int Poids { get; set; }

        [Column("jou_taille"), Required]
        [Range(0, int.MaxValue, ErrorMessage = "La taille du joueur doit être positive.")]
        public int Taille { get; set; }


        [Column("jou_biographie"), Required]
        [StringLength(MAX_BIO_LENGTH, ErrorMessage = "La biographie ne doit pas dépasser 500 caractères.")]
        public string Biographie { get; set; } = null!;

        [Column("jou_imageurl")]
        public string? ImageUrl { get; set; } = null!;

        //Statistiques
        [InverseProperty(nameof(Statistiques.Joueur))]
        public Statistiques? Stats { get; set; }

        //Club
        [Column("clb_id"), Required]
        public int IdClub {  get; set; }

        [ForeignKey(nameof(IdClub))]
        public Club Club { get; set; }

        //Pays
        [Column("pys_id"), Required]
        public int IdPays { get; set; }

        [ForeignKey(nameof(IdPays))]
        public Pays Pays { get; set; }

        [Column("jou_pied"), Required]
        public PiedJoueur Pied { get; set; }

        [Column("jou_poste"), Required]
        public PosteJoueur Poste { get; set; }

        [InverseProperty(nameof(FaqJoueur.Joueur))]
        public ICollection<FaqJoueur> FaqJoueurs { get; set; } = new HashSet<FaqJoueur>();

        [ManyToMany(nameof(Trophee.Joueurs))]
        public ICollection<Trophee> Trophees { get; set; } = new HashSet<Trophee>();

        [ManyToMany(nameof(Publication.Joueurs)), JsonIgnore]
        public ICollection<Publication> Publications { get; set; } = new HashSet<Publication>();
    }
}
