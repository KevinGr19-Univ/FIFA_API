using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_album_alb")]
	public class Album
	{
		[Column("alb_id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column("alb_idphoto")]
		public int IdPhoto { get; set; }

		[Column("alb_titre")]
		[StringLength(100,  ErrorMessage = "Le titre ne doit pas dépasser 100 caractères")]
		public string Titre { get; set; }

		[Column("alb_resume")]
		[StringLength(500, ErrorMessage = "Le résumé ne doit pas dépasser 500 caractères")]
		public string Resume { get; set; }

		[Column("alb_datepublication", TypeName = "date")]
		public DateTime DatePublication { get; set; }
	}
}