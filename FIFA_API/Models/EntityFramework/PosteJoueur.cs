using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_r_postejoueur_poj")]
    [Index(nameof(NomPoste), IsUnique = true)]
    public partial class PosteJoueur
    {
        public const int MAX_NOMPOSTE_LENGTH = 60;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("poj_id")]
        public int Id { get; set; }

        [Column("poj_nomposte")]
        [StringLength(MAX_NOMPOSTE_LENGTH, ErrorMessage = "Le nom de poste ne doit pas dépasser 60 caractères.")]
        public string NomPoste { get; set; }

        [InverseProperty(nameof(Joueur.Poste))]
        public virtual ICollection<Joueur> Joueurs { get; set; }
    }
}
