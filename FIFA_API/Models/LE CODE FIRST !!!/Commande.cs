using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.LE_CODE_FIRST____
{
	[Table("t_e_commande_cmd")]
    public class Commande
    {
        public TypeLivraison TypeLivraison { get; set; }
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


        public ICollection<LigneCommande> Lignes { get; set; }
        public ICollection<StatusCommande> Status { get; set; }
    }
}
