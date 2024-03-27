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

        [Column("tli_id"), Required]
        public int IdTypeLivraison { get; set; }

        [ForeignKey(nameof(IdTypeLivraison))]
        public TypeLivraison TypeLivraison { get; set; }

        [Column("utl_id"), Required]
        public int IdUtilisateur { get; set; }

        [ForeignKey(nameof(IdUtilisateur))]
        public virtual Utilisateur Utilisateur { get; set; }

        [Column("adr_livraison_id"), Required]
        public int IdAdresseLivraison { get; set; }

        [ForeignKey(nameof(IdAdresseLivraison))]
        //[Column("cmd_adresselivraison"), Required]
        public Adresse AdresseLivraison { get; set; }

        [Column("adr_facuration_id"), Required]
        public int IdAdresseFacturation { get; set; }

        [ForeignKey(nameof(IdAdresseFacturation))]
        //[Column("cmd_adressefacturation"), Required]
        public Adresse AdresseFacturation { get; set; }

		[Column("cmd_prixlivraison"), Required]
        [Precision(7,2)]
        public decimal PrixLivraison { get; set; }

		[Column("cmd_datecommande"), Required]
        public DateTime DateCommande { get; set; }

		[Column("cmd_dateexpedition")]
        public DateTime? DateExpedition { get; set; }

		[Column("cmd_datelivraison")]
        public DateTime? DateLivraison { get; set; }

		[Column("cmd_urlfacture", TypeName ="text"), Required]
        public string UrlFacture { get; set; }

        [InverseProperty(nameof(LigneCommande.Commande))]
        public ICollection<LigneCommande> Lignes { get; set; }

        [InverseProperty(nameof(StatusCommande.Commande))]
        public ICollection<StatusCommande> Status { get; set; }
    }
}
