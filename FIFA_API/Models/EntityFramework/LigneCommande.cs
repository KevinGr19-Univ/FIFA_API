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

		[Column("vcp_id")]
		[ForeignKey("")]
		public int IdVarianteCouleurProduit { get; set; }

        [Column("tlp_id")]
        [ForeignKey("")]
        public int IdTailleProduit { get; set; }

        [Column("cmd_id")]
        [ForeignKey("")]
        public int IdCommande { get; set; }

		[Column("lco_quantite")]
		public int Quantite { get; set; }

        [Column("lco_prix", TypeName="Numeric(10,2)")]
        public int Prix { get; set; }

		[Column("lco_prixpromotion", TypeName = "Numeric(10,2)")]
        public int PrixPromotion { get; set; }


    }
}