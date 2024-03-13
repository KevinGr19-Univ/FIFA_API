using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_statistiques_stt")]
    public partial class Statistiques
    {
        [Key, Column("jou_id")]
        public int IdJoueur { get; set; }

		[Column("stt_matchsjoues")]
        public int MatchsJoues { get; set; }

		[Column("stt_titularisations")]
        public int Titularisations { get; set; }

		[Column("stt_minutesjouees")]
        public int MinutesJouees { get; set; }

		[Column("stt_buts")]
        public int Buts { get; set; }

        [ForeignKey(nameof(IdJoueur))]
        public Joueur Joueur { get; set; }
    }
}
