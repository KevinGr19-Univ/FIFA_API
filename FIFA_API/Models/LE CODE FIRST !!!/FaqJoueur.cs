using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class FaqJoueur
    {

        public Joueur Joueur { get; set; }

        public string Question { get; set; } 

        public string Reponse { get; set; } 
    }
}
