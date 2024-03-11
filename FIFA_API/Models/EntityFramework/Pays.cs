using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_pays_pys")]
	public class Pays
	{
		[Column("pys_id")]
		[Key()]
		public int Id { get; set; }

		[Column("pys_nom")]
		[StringLength(100, ErrorMessage = "Le nom du pays ne doit pas dépasser les 100 caractères")]
		public string Nom { get; set; }
	}
}