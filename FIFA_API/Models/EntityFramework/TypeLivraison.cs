using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_typelivraison_tli")]
    public class TypeLivraison
    {
        public TypeLivraison()
        {
            Commandes = new HashSet<Commande>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tli_id")]
        public int Id { get; set; }

        [StringLength(50,ErrorMessage = "Le nom ne doit pas d�passer les 50 caract�res")]
        [Column("tli_nom")]
        public string Nom { get; set; }

		[Column("tli_maxbusinessdays")]
        public int MaxBusinessDays { get; set; }

		[Column("tli_prix")]
        [Precision(7,2)]
        public decimal Prix { get; set; }

        [InverseProperty(nameof(Commande.TypeLivraison))]
        public virtual ICollection<Commande> Commandes { get; set; }
    }
}
