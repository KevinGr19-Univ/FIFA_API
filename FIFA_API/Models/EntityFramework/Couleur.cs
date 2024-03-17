using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIFA_API.Models.Utils;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_e_couleur_col")]
    public partial class Couleur
    {
        public const int MAX_NOM_LENGTH = 50;

        public Couleur()
        {
            Produits = new HashSet<Produit>();
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

        public virtual ICollection<Produit> Produits { get; set; }
    }
}
