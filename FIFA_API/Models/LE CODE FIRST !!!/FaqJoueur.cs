using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    public abstract class FaqJoueur
    {
        public int Id { get; set; }

        public Joueur joueur { get; set; }

        public string Question { get; set; } 

        public string Reponse { get; set; } 
    }
}
