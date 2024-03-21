
using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{

	[Table("t_e_voteutilisateur_vtl")]
    [ComposedKey(nameof(IdUtilisateur), nameof(IdTheme))]
    public partial class VoteUtilisateur
    {
        [Column("utl_id", Order = 0), Required]
        public int IdUtilisateur { get; set; }

        [Column("thv_id", Order = 1), Required]
        public int IdTheme { get; set; }

        [Column("jou_id1"), Required]
        public int IdJoueur1 { get; set; }

        [Column("jou_id2"), Required]
        public int IdJoueur2 { get; set; }

        [Column("jou_id3"), Required]
        public int IdJoueur3 { get; set; }

        [ForeignKey(nameof(IdUtilisateur))]
        public virtual Utilisateur Utilisateur { get; set; }

        [ForeignKey(nameof(IdTheme))]
        public ThemeVote ThemeVote { get; set; }

        [ForeignKey(nameof(IdJoueur1))]
        public virtual Joueur Joueur1 { get; set; }

        [ForeignKey(nameof(IdJoueur2))]
        public virtual Joueur Joueur2 { get; set; }

        [ForeignKey(nameof(IdJoueur3))]
        public virtual Joueur Joueur3 { get; set; }
    }
}
