using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_lignecommande_lco")]
	public class LigneCommande
	{
		[Column("lco_id")]
		[Key()]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column("lco_idvariantecouleurproduit")]
		[ForeignKey("")]
		public int IdVarianteCouleurProduit { get; set; }

        [Column("lco_idtailleproduit")]
        [ForeignKey("")]
        public int IdTailleProduit { get; set; }

        [Column("lco_idcommande")]
        [ForeignKey("")]
        public int IdCommande { get; set; }

		[Column("lco_quantite")]
		public int Quantite { get; set; }

        [Column("lco_prix", TypeName="Numeric(10,2)")]
        public int Prix { get; set; }


    }
}