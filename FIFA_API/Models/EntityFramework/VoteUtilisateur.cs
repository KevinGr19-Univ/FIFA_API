
using FIFA_API.Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{

	[Table("t_e_voteutilisateur_vtl")]
    [ComposedKey(nameof(IdUtilisateur), nameof(NomTheme))]
    public partial class VoteUtilisateur
    {
        [Column("utl_id", Order = 0), Required]
        public int IdUtilisateur { get; set; }

        [Column("thv_nomtheme", Order = 1), Required]
        public string NomTheme { get; set; }

        [Column("jou_id1"), Required]
        public int IdJoueur1 { get; set; }

        [Column("jou_id2"), Required]
        public int IdJoueur2 { get; set; }

        [Column("jou_id3"), Required]
        public int IdJoueur3 { get; set; }

        [ForeignKey(nameof(IdUtilisateur))]
        public Utilisateur Utilisateur { get; set; }

        [ForeignKey(nameof(NomTheme))]
        public ThemeVote ThemeVote { get; set; }

        [ForeignKey(nameof(IdJoueur1))]
        public Joueur Joueur1 { get; set; }

        [ForeignKey(nameof(IdJoueur2))]
        public Joueur Joueur2 { get; set; }

        [ForeignKey(nameof(IdJoueur3))]
        public Joueur Joueur3 { get; set; }
    }
}
