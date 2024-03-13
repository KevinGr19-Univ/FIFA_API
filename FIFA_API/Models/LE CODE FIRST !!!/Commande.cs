using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_commande_cmd")]
    public class Commande
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cmd_id")]
        public int Id { get; set; }

        [Column("tli_id")]
        public int IdTypeLivraison { get; set; }

        [ForeignKey(nameof(IdTypeLivraison))]
        public TypeLivraison TypeLivraison { get; set; }

        [Column("utl_id")]
        public int IdUtilisateur { get; set; }

        [ForeignKey(nameof(IdUtilisateur))]
        public Utilisateur Utilisateur { get; set; }

        public Adresse AdresseLivraison { get; set; }
        public Adresse AdresseFacturation { get; set; }

		[Column("cmd_prixlivraison")]
        public decimal PrixLivraison { get; set; }

		[Column("cmd_dateexpedition")]
        public DateTime DateExpedition { get; set; }

		[Column("cmd_datecommande")]
        public DateTime? DateCommande { get; set; }

		[Column("cmd_datelivraison")]
        public DateTime? DateLivraison { get; set; }

		[Column("cmd_urlfacture")]
        public string UrlFacture { get; set; }

        [Column("lco_id")]
        public int IdLigneCommande { get; set; }

        [ForeignKey(nameof(IdLigneCommande))]
        public ICollection<LigneCommande> Lignes { get; set; }

        [Column("sco_id")]
        public int IdStatusCommande { get; set; }

        [ForeignKey(nameof(IdStatusCommande))]
        public ICollection<StatusCommande> Status { get; set; }
    }
}
