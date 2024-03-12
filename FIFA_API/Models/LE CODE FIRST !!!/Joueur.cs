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
        ardienDeBut = 2,
        MilieuDeTerrain = 3
    }

	[Table("t_e_joueur_jou")]
    public class Joueur
    {
		[Column("jou_nomjoueur")]
        public string NomJoueur { get; set; } = null!;

		[Column("jou_prenomjoueur")]
        public string PrenomJoueur { get; set; } = null!;

		[Column("jou_datenaissancejoueur")]
        public DateTime? DateNaissanceJoueur { get; set; }

		[Column("jou_lieunaissancejoueur")]
        public string LieuNaissanceJoueur { get; set; } = null!;

        public Pied PiedJoueur { get; set; }

		[Column("jou_poidsjoueur")]
        public int PoidsJoueur { get; set; }

		[Column("jou_taillejoueur")]
        public int TailleJoueur { get; set; }

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
