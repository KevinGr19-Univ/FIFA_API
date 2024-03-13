using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.LE_CODE_FIRST____
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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("jou_id")]
        public int Id { get; set; }

        [Column("jou_nom")]
        public string Nom { get; set; } = null!;

		[Column("jou_prenom")]
        public string Prenom { get; set; } = null!;

		[Column("jou_datenaissance")]
        public DateTime? DateNaissance { get; set; }

		[Column("jou_lieunaissance")]
        public string LieuNaissance { get; set; } = null!;

        public Pied Pied { get; set; }

		[Column("jou_poids")]
        public int Poids { get; set; }

		[Column("jou_taille")]
        public int Taille { get; set; }

        public Poste PosteJoueur { get; set; } 

		[Column("jou_biographie")]
        public string Biographie { get; set; } = null!;

        public Photo Photo { get; set; } = null!;
        
        public Statistiques Stats { get; set; }
        public Club Club { get; set; }
        public Pays Pays { get; set; }

        public ICollection<Trophee> Trophees { get; set; }
    }
}
