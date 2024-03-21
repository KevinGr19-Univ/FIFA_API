using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIFA_API.Models.Utils;
using FIFA_API.Utils;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_couleur_col")]
    public partial class Couleur
    {
        public const int MAX_NOM_LENGTH = 50;

        public Couleur()
        {
            VariantesProduits = new HashSet<VarianteCouleurProduit>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("col_id")]
        public int Id { get; set; }

        [Column("col_nom"), Required]
        [StringLength(MAX_NOM_LENGTH, ErrorMessage = "Le nom de la couleur ne doit pas dépasser 50 caractères.")]
        public string Nom { get; set; }

		[Column("col_codehexa"), Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Le code hexa doit faire 6 caractères de long.")]
        [RegularExpression(ModelUtils.REGEX_HEXACOLOR, ErrorMessage = "Le code hexadécimal de couleur doit être au format hexadécimal.")]
        public string CodeHexa { get; set; }

        [InverseProperty(nameof(VarianteCouleurProduit.Couleur))]
        public virtual ICollection<VarianteCouleurProduit> VariantesProduits { get; set; }
    }
}
