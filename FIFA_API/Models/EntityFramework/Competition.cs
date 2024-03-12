using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_competition_cmp")]
    public class Competition
    {
        public ICollection<Produit> Produits { get; set; }

        [Column("cmp_id")]
        [Key()]
        public int Id { get; set; }


        [Column("cmp_nom")]
        [StringLength(100, ErrorMessage = "Le nom de la compétition ne doit pas dépasser les 100 caractères")]
        public String Nom { get; set; }
    }
}
