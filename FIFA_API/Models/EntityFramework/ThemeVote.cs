using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_ThemeVote_tmv")]
	public partial class ThemeVote
	{
        [Key]
        [Column("tmv_id")]
        public int Id { get; set; }

        [Column("tmv_titre")]
        [StringLength(100)]
        public string Titre { get; set; }

    }
}