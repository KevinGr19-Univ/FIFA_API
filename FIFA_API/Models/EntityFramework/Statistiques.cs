using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_statistiques_stt")]
    public partial class Statistiques
    {
        [Key, Column("jou_id"), Required]
        public int IdJoueur { get; set; }

		[Column("stt_matchsjoues"), Required]
        public int MatchsJoues { get; set; }

		[Column("stt_titularisations"), Required]
        public int Titularisations { get; set; }

		[Column("stt_minutesjouees"), Required]
        public int MinutesJouees { get; set; }

		[Column("stt_buts"), Required]
        public int Buts { get; set; }

        [ForeignKey(nameof(IdJoueur))]
        public virtual Joueur Joueur { get; set; }
    }
}
