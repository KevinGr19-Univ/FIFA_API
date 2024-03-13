using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_statistiques_stt")]
    public partial class Statistiques
    {
        [Key]
        [Column("stt_id")]
        public int Id { get; set; }
        public Joueur Joueur { get; set; }

		[Column("stt_matchsjoues")]
        public int MatchsJoues { get; set; }

		[Column("stt_titularisations")]
        public int Titularisations { get; set; }

		[Column("stt_minutesjouees")]
        public int MinutesJouees { get; set; }

		[Column("stt_buts")]
        public int Buts { get; set; }

    }
}
