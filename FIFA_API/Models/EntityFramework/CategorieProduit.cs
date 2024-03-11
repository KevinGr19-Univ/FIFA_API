using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_categorieproduit_ctp")]
	public partial class CategorieProduit
	{
        [Key]
        [Column("ctp_id")]
		public int Id { get; set; }

		[Column("ctp_nom")]
		public string Nom { get; set; }

	}
}