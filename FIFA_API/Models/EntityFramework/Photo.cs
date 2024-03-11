using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_photo_pht")]
	public partial class Photo
	{
        [Key]
        [Column("pht_id")]
        public int Id { get; set; }

        [Column("pht_nom")]
        [StringLength(50)]
        public string Nom { get; set; }

        [Column("pht_url")]
        [StringLength(500)]
        public string Url { get; set; }

    }
}