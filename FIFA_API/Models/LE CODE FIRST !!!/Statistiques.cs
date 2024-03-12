using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public abstract class StatistiquesJoueur
    {
 
        public int Id { get; set; }
        public Joueur Joueur { get; set; }
        public int MatchsJoues { get; set; }
        public int Titularisations { get; set; }
        public int MinutesJouees { get; set; }
        public int Buts { get; set; }
    }
}
