using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_nation_ntn")]
	public class Nation
	{
		[Column("ntn_id")]
		[Key()]
		public int Id { get; set; }

		[Column("ntn_nom")]
		[StringLength(100, ErrorMessage = "Le nom de la nation ne doit dépasser les 100 caractères")]
		public string Nom { get; set; }
	}
}