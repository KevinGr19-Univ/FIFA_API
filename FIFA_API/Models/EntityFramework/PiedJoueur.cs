using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_r_piedjoueur_pjo")]
    [Index(nameof(Libelle), IsUnique = true)]
    public partial class PiedJoueur
    {
        public const int MAX_LIBELLE_LENGTH = 40;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pjo_id")]
        public int Id { get; set; }

        [Column("pjo_libelle")]
        [StringLength(MAX_LIBELLE_LENGTH, ErrorMessage = "Le libellé ne doit pas dépasser 40 caractères.")]
        public string Libelle { get; set; }

        [InverseProperty(nameof(Joueur.Pied))]
        public virtual ICollection<Joueur> Joueurs { get; set; }
    }
}
