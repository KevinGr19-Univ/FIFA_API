using FIFA_API.Models.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace FIFA_API.Models.EntityFramework
{
	[Table("t_e_commande_cmd")]
    [Index(nameof(UrlFacture),IsUnique =true)]
    public partial class Commande
    {
        public const int MAX_VILLE_LENGTH = 150;
        public const int MAX_RUE_LENGTH = 200;

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

        [Column("cmd_villelivraison"), Required]
        [StringLength(MAX_VILLE_LENGTH, ErrorMessage = "Le nom de la ville ne doit pas dépasser les 150 caractères")]
        public string VilleLivraison { get; set; }

        [Column("cmd_ruelivraison"), Required]
        [StringLength(MAX_RUE_LENGTH, ErrorMessage = "Le nom de la rue ne doit pas dépasser les 200 caractères")]
        public string RueLivraison { get; set; }

        [Column("cmd_codepostallivraison"), Required]
        [StringLength(5, MinimumLength = 5)]
        [RegularExpression(ModelUtils.REGEX_CODEPOSTAL, ErrorMessage = "Le code postal ne respecte pas les normes françaises")]
        public string CodePostalLivraison { get; set; }

        [Column("cmd_villefacturation"), Required]
        [StringLength(MAX_VILLE_LENGTH, ErrorMessage = "Le nom de la ville ne doit pas dépasser les 150 caractères")]
        public string VilleFacturation { get; set; }

        [Column("cmd_ruefacturation"), Required]
        [StringLength(MAX_RUE_LENGTH, ErrorMessage = "Le nom de la rue ne doit pas dépasser les 200 caractères")]
        public string RueFacturation { get; set; }

        [Column("cmd_codepostalfacturation"), Required]
        [StringLength(5, MinimumLength = 5)]
        [RegularExpression(ModelUtils.REGEX_CODEPOSTAL, ErrorMessage = "Le code postal ne respecte pas les normes françaises")]
        public string CodePostalFacturation { get; set; }

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
