using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
    [Table("t_j_themevotejoueur_thj")]
    [ComposedKey(nameof(IdTheme), nameof(IdJoueur))]
    public class ThemeVoteJoueur
    {
        private readonly ILazyLoader _loader;

        public ThemeVoteJoueur() { }
        public ThemeVoteJoueur(ILazyLoader loader)
        {
            _loader = loader;
        }

        [Column("thv_id", Order = 0)]
        public int IdTheme { get; set; }

        [Column("jou_id", Order = 1)]
        public int IdJoueur { get; set; }

        private ThemeVote _theme;
        private Joueur _joueur;

        [ForeignKey(nameof(IdTheme))]
        [OnDelete(DeleteBehavior.Cascade)]
        public ThemeVote Theme
        {
            get => _loader.Load(this, ref _theme);
            set => _theme = value;
        }

        [ForeignKey(nameof(IdJoueur))]
        [OnDelete(DeleteBehavior.Cascade)]
        public Joueur Joueur
        {
            get => _loader.Load(this, ref _joueur);
            set => _joueur = value;
        }
    }
}
