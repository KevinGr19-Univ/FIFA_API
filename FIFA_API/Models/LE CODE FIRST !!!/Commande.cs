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

        [Column("adr_id1")]
        public int IdAdresseLivraison { get; set; }

        [ForeignKey(nameof(IdAdresseLivraison))]
        public Adresse AdresseLivraison { get; set; }
        
        [Column("adr_id2")]
        public int IdAdresseFacturation { get; set; }

        [ForeignKey(nameof(IdAdresseFacturation))]
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

        public ICollection<LigneCommande> Lignes { get; set; }

        public ICollection<StatusCommande> Status { get; set; }
    }
}
