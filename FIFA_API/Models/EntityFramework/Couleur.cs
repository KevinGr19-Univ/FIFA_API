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
        [StringLength(100, ErrorMessage = "Le nom ne doit pas d�passer 100 caract�res")]
        public string Nom { get; set; } = null!;

        [Column("clr_hexacouleur", TypeName = "char")]
        [StringLength(6, ErrorMessage = "La couleur doit �tre �crite en Hexa, et doit mesurer 6 caract�res")]
        [RegularExpression(ModelUtils.REGEX_HEXACOLOR)]
        public string HexaCouleur { get; set; } = null!;
    }
}