using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class Produit
    {
        public Competition Competition { get; set; }
        public Nation Nation { get; set; }
        public Genre Genre { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }

        public ICollection<Produit> Associes { get; set; }
        public ICollection<Couleur> Couleurs { get; set; }
    }
}
