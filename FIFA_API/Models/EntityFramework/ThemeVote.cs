using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_themevote_tmv")]
	public partial class ThemeVote
	{
        [Key]
        [Column("tmv_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("tmv_titre")]
        [StringLength(100, ErrorMessage = "Le Titre ne doit pas dépasser 100 caractères")]
        public string Titre { get; set; }

    }
}