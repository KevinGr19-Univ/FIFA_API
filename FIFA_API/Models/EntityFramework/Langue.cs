using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_langue_lng")]
    public class Langue
	{
		[Column("lng_id")]
		[Key()]
		public int Id { get; set; }

		[Column("lng_nom")]
		[StringLength(100, ErrorMessage = "La longueur du nom de langue ne doit pas dépasser 100 caractères")]
		public string Nom { get; set; }
	}
}