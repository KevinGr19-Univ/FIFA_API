using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_article_art")]
	public class Article
	{
        [Column("art_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("art_idphoto")]
        public int IdPhoto { get; set; }

        [Column("art_titre")]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas d�passer 100 caract�res")]
        public string Titre { get; set; }

        [Column("art_resume")]
        [StringLength(500, ErrorMessage = "Le r�sum� ne doit pas d�passer 500 caract�res")]
        public string Resume { get; set; }

        [Column("art_datepublication", TypeName = "date")]
        public DateTime DatePublication { get; set; }

        [Column("art_texte")]
        public string TexteArticle { get; set; }
    }
}