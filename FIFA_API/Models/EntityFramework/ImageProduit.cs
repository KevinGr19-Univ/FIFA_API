using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_imageproduit_img")]
    public partial class ImageProduit
	{
        [Key]
        [Column("img_id")]
        public int Id { get; set; }

        [Column("doc_url")]
        [StringLength(500, ErrorMessage = "L'URL du document ne doit pas d�passer 500 caract�res")]
        public string UrlImageProduit{ get; set; } = null!;
    }
}