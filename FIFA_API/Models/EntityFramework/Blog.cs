using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_blog_blg")]
	public class Blog
	{
        [Column("blg_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("blg_idphoto")]
        public int IdPhoto { get; set; }

        [Column("blg_titre")]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas d�passer 100 caract�res")]
        public string Titre { get; set; }

        [Column("blg_resume")]
        [StringLength(500, ErrorMessage = "Le r�sum� ne doit pas d�passer 500 caract�res")]
        public string Resume { get; set; }

        [Column("blg_datepublication", TypeName = "date")]
        public DateTime DatePublication { get; set; }

        [Column("blg_texte")]
        public string TexteBlog { get; set; }
    }
}