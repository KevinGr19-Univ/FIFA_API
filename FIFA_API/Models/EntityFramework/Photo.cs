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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("pht_nom")]
        [StringLength(50, ErrorMessage = "Le Nom ne doit pas dépasser 50 caractères")]
        public string Nom { get; set; }

        [Column("pht_url")]
        [StringLength(500, ErrorMessage = "L'url ne doit pas dépasser 500 caractères")]
        public string Url { get; set; }

    }
}