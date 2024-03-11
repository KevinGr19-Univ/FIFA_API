using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_variantecouleurproduit_vcp")]
	public partial class VarianceCouleurProduit
	{
		[Key]
		[Column("vcp_id")]
		public int Id { get; set; }

		[Column("vcp_idproduit")]
		public int IdProduit { get; set; }

		[Column("vcp_idcouleur")]
		public int IdCouleur { get; set; }

		[Column("vcp_prix")]
		[Precision(2)]
		public double Prix { get; set; }
	}
}