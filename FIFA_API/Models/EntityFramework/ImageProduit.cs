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
        [StringLength(500)]
        public string UrlImageProduit{ get; set; } = null!;
    }
}