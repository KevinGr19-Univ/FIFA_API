using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_produit_pro")]
	public partial class Produit
	{
        [Key]
        [Column("pro_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Foreing Key
        [Column("pro_idcompetition")]
        public int IdCompetition { get; set; }

        //Foreing Key
        [Column("pro_idnation")]
        public int IdNation { get; set; }

        //Foreing Key
        [Column("pro_idgenre")]
        public int IdGenre { get; set; }

        [Column("pro_titre")]
        [StringLength(150, ErrorMessage = "Le Titre ne doit pas dépasser 150 caractères")]
        public string Titre { get; set; }

        [Column("pro_description")]
        public string Description { get; set; }


    }
}