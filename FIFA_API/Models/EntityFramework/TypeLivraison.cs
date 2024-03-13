using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_typelivraison_tli")]
    public class TypeLivraison
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tli_id")]
        public int Id { get; set; }
        [Column("tli_nom")]
        public string Nom { get; set; }

		[Column("tli_maxbusinessdays")]
        public int MaxBusinessDays { get; set; }

		[Column("tli_prix")]
        public decimal Prix { get; set; }

        public virtual ICollection<Commande> Commandes { get; set; }
    }
}
