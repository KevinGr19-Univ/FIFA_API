using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_j_themevotejoueur_thj")]
    [ComposedKey(nameof(IdTheme), nameof(IdJoueur))]
    public partial class ThemeVoteJoueur
    {
        [Column("thv_id", Order = 0)]
        public int IdTheme { get; set; }

        [Column("jou_id", Order = 1)]
        public int IdJoueur { get; set; }

        [ForeignKey(nameof(IdTheme))]
        [OnDelete(DeleteBehavior.Cascade)]
        public ThemeVote Theme { get; set; }

        [ForeignKey(nameof(IdJoueur))]
        [OnDelete(DeleteBehavior.Cascade)]
        public Joueur Joueur { get; set; }
    }
}
