using FIFA_API.Models.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
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
    [ComposedKey(nameof(IdCommande), nameof(Code))]
    public partial class StatusCommande
    {
        public const int MAX_COMMENTAIRE_LENGTH = 500;

        [Column("cmd_id"), Required]
        public int IdCommande { get; set; }

		[Column("sco_date"), Required]
        public DateTime Date { get; set; }

		[Column("sco_commentaire")]
        [StringLength(MAX_COMMENTAIRE_LENGTH, ErrorMessage = "Le commentaire ne doit pas dépasser 500 caractères.")]
        public string? Commentaire { get; set; }

        [ForeignKey(nameof(IdCommande))]
        public Commande Commande { get; set; }

        [Column("sco_code"), Required]
        public CodeStatusCommande Code { get; set; }
    }
}
