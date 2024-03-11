using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_tailleproduit_tlp")]
	public partial class TailleProduit
	{
        [Key]
        [Column("tlp_id")]
        public int Id { get; set; }

        [Column("tlp_nom")]
        [StringLength(50)]
        public string Nom { get; set; }


    }
}