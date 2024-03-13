using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.EntityFramework
{

    
    public enum Pied
    {
        Droitier = 0,
        Gaucher = 1,
        Ambidextre = 2
    }

    public enum Poste
    {
        Attaquant = 0,
        Defenseur = 1,
        GardienDeBut = 2,
        MilieuDeTerrain = 3
    }

    [Table("t_e_joueur_jou")]
    public class Joueur
    {

        public const int MAX_LENGTH = 100;
        public const int MAX_LENGTH_BIO = 500;


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("jou_id")]
        public int Id { get; set; }


        [Column("jou_nom")]
        [StringLength(MAX_LENGTH, ErrorMessage = "Le nom ne doit pas dépasser 100 caractères.")]
        public string Nom { get; set; } = null!;



        [Column("jou_prenom")]
        [StringLength(MAX_LENGTH, ErrorMessage = "Le prénom ne doit pas dépasser 100 caractères.")]
        public string Prenom { get; set; } = null!;



        [Column("jou_datenaissance", TypeName = "date")]
        public DateTime? DateNaissance { get; set; }


        [StringLength(MAX_LENGTH, ErrorMessage = "Le lieu de naissance ne doit pas dépasser 100 caractères.")]
        [Column("jou_lieunaissance")]
        public string LieuNaissance { get; set; } = null!;



        [Column("jou_pied")]
        public Pied Pied { get; set; }



        [Column("jou_poids")]
        [Range(0, int.MaxValue, ErrorMessage = "Le poids du joueur doit être positif.")]
        public int Poids { get; set; }



        [Column("jou_taille")]
        [Range(0, int.MaxValue, ErrorMessage = "La taille du joueur doit être positive.")]
        public int Taille { get; set; }



        [Column("jou_poste")]
        public Poste PosteJoueur { get; set; }



        [Column("jou_biographie")]
        [StringLength(MAX_LENGTH_BIO, ErrorMessage = "La biographie ne doit pas dépasser 500 caractères.")]
        public string Biographie { get; set; } = null!;




        //Photo
        [Column("pht_id")]
        public int IdPhoto { get; set; }

        [ForeignKey(nameof(IdPhoto))]
        public Photo Photo { get; set; } = null!;


        //Statistiques
        [Column("stt_idjoueur")]
        public int IdJoueur { get; set; }

        [ForeignKey(nameof(IdJoueur))]
        public Statistiques Stats { get; set; }


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



        [InverseProperty(nameof(FaqJoueur.Joueur))]
        public ICollection<FaqJoueur> FaqJoueurs { get; set; }

        public ICollection<Trophee> Trophees { get; set; }
    }
}
