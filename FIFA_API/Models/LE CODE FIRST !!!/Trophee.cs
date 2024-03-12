using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_trophee_tph")]
    public class Trophee
    {
		[Column("tph_nom")]
        public string Nom { get; set; }

        public ICollection<Joueur> Joueurs { get; set; }
    }
}
