using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public enum Pied { Droitier = 0, Gaucher = 1, Ambidextre = 2 }
    public enum Poste { Attaquant = 0, Defenseur = 1, GardienDeBut = 2, MilieuDeTerrain = 3 }

    public class Joueur
    {
        public string NomJoueur { get; set; } = null!;
        public string PrenomJoueur { get; set; } = null!;
        public DateTime? DateNaissanceJoueur { get; set; }
        public string LieuNaissanceJoueur { get; set; } = null!;
        public Pied PiedJoueur { get; set; }
        public int PoidsJoueur { get; set; }
        public int TailleJoueur { get; set; }
        public Poste PosteJoueur { get; set; } 
        public string Biographie { get; set; } = null!;
        public Photo Photo { get; set; } = null!;
        
        public Statistiques Stats { get; set; }
        public Club Club { get; set; }
        public Pays Pays { get; set; }

        public ICollection<Trophee> Trophees { get; set; }
    }
}
