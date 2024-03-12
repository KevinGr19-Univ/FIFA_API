using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    public abstract class StatistiquesJoueur
    {
 
        public int Id { get; set; }
        public int JoueurId { get; set; }
        public int MatchsJoues { get; set; }
        public int Titularisations { get; set; }
        public int MinutesJouees { get; set; }
        public int Buts { get; set; }
    }
}
