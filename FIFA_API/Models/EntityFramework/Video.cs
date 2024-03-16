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
        [StringLength(500, ErrorMessage = "Le nom de la vidéo ne doit pas dépasser les 500 caractères")]
        public string Nom { get; set; }

        [StringLength(500, ErrorMessage = "L'url de la vidéo ne doit pas dépasser les 500 caractères")]
        [Column("vid_url")]
        public string Url { get; set; }

        #region Many-to-many
        private ICollection<Article> _articles { get; set; }
        #endregion
    }
}
