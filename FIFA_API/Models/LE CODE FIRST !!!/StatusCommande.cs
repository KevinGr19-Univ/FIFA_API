using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public enum CodeStatusCommande
    {
        Preparation = 0,
        Validation = 1,
        Expedition = 2,
        Livre = 3,
        Annule = 4,
        Refuse = 5,
        RefusAccepte = 6,
    }

	[Table("t_j_statuscommande_sco")]
    public class StatusCommande
    {
        public Commande Commande { get; set; }
        public CodeStatusCommande Code { get; set; }

		[Column("sco_date")]
        public DateTime Date { get; set; }

		[Column("sco_commentaire")]
        public string Commentaire { get; set; }

    }
}
