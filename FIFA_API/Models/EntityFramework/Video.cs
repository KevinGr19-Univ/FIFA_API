using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_video_vid")]
    public partial class Video
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("vid_id")]
        public int Id { get; set; }

        [Column("vid_nom")]
        [StringLength(500, ErrorMessage = "Le nom de la vid�o ne doit pas d�passer les 500 caract�res")]
        public string Nom { get; set; }

        [StringLength(500, ErrorMessage = "L'url de la vid�o ne doit pas d�passer les 500 caract�res")]
        [Column("vid_url")]
        public string Url { get; set; }

        #region Many-to-many
        private ICollection<Article> _articles { get; set; }
        #endregion
    }
}
