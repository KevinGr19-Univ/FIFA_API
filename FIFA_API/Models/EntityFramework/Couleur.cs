using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_couleur_clr")]
	public partial class Couleur
	{
        [Key]
        [Column("clr_id")]
        public int Id { get; set; }

        [Column("clr_nom")]
        [StringLength(100)]
        public string Nom { get; set; } = null!;

        [Column("clr_hexacouleur", TypeName = "char")]
        [StringLength(6)]
        [RegularExpression(ModelUtils.REGEX_HEXACOLOR)]
        public string HexaCouleur { get; set; } = null!;
    }
}