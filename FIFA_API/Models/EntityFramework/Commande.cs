using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_commande_cmd")]
    [Index(nameof(UrlFacture),IsUnique =true)]
    public partial class Commande
    {
        public Commande()
        {
            Lignes = new HashSet<LigneCommande>();
            Status = new HashSet<StatusCommande>();
        }

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

        [Column("adr_livraison_id")]
        public int IdAdresseLivraison { get; set; }

        [ForeignKey(nameof(IdAdresseLivraison))]
        public Adresse AdresseLivraison { get; set; }
        
        [Column("adr_facuration_id")]
        public int IdAdresseFacturation { get; set; }

        [ForeignKey(nameof(IdAdresseFacturation))]
        public Adresse AdresseFacturation { get; set; }

		[Column("cmd_prixlivraison")]
        [Precision(7,2)]
        public decimal PrixLivraison { get; set; }

		[Column("cmd_datecommande")]
        public DateTime DateCommande { get; set; }

		[Column("cmd_dateexpedition")]
        public DateTime? DateExpedition { get; set; }

		[Column("cmd_datelivraison")]
        public DateTime? DateLivraison { get; set; }

		[Column("cmd_urlfacture", TypeName ="text")]
        public string UrlFacture { get; set; }

        [InverseProperty(nameof(LigneCommande.Commande))]
        public virtual ICollection<LigneCommande> Lignes { get; set; }

        [InverseProperty(nameof(StatusCommande.Commande))]
        public virtual ICollection<StatusCommande> Status { get; set; }
    }
}
