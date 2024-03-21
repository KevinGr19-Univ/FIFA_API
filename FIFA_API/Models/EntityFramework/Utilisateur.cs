using FIFA_API.Models.Annotations;
using FIFA_API.Models.Utils;
using FIFA_API.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FIFA_API.Models.EntityFramework
{
    ////Ajouter les roles
    //public enum RoleUtilisateur
    //{
    //    Utilisateur = 0,
    //    DirecteurDuServiceVentes = 1,
    //    MembreDuServiceExpedition = 2,
    //    MembreDuServiceCommande = 3
    //}

    [Table("t_e_utilisateur_utl")]
    [Index(nameof(Mail), IsUnique = true)]
    public partial class Utilisateur
    {
        public Utilisateur()
        {
            Commandes = new HashSet<Commande>();
            Votes = new HashSet<VoteUtilisateur>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("utl_id")]
        public int Id { get; set; }

        [Column("utl_prenom")]
        [StringLength(100, ErrorMessage = "Le prénom ne doit pas dépasser les 100 caractères")]
        public string? Prenom { get; set; }

        [Column("utl_surnom")]
        [StringLength(100, ErrorMessage = "Le surnom ne doit pas dépasser les 100 caractères")]
        public string? Surnom { get; set; }

        [Column("utl_telephone")]
        [RegularExpression(ModelUtils.REGEX_TELEPHONE, ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres, dont le 1er doit être un 0")]
        public string? Telephone { get; set; }

        [Column("utl_mail"), Required]
        [EmailAddress(ErrorMessage = "L'addresse email doit convenir aux normes des adresses email")]
        public string Mail { get; set; }

        [Column("utl_stripeid")]
        [StringLength(100, ErrorMessage = "L'id de stripe ne doit pas dépasser les 100 caractères")]
        public string? StripeId { get; set; }

        [Column("utl_refreshtoken")]
        public string? RefreshToken { get; set; }

		[Column("utl_datenaissance", TypeName = "date")]
        public DateTime? DateNaissance { get; set; }

        [Column("utl_hashpwd"), Required]
        public string HashMotDePasse { get; set; }

		[Column("utl_derniereconnexion")]
        public DateTime? DerniereConnexion { get; set; }

		[Column("utl_dateverificationemail")]
        public DateTime? DateVerificationEmail { get; set; }

		[Column("utl_doubleauthentification")]
        public bool DoubleAuthentification { get; set; }

        // Role
        [Column("utl_role")]
        [StringLength(200)]
        public string? Role { get; set; }

        // Langue
        [Column("lan_id"), Required]
        public int IdLangue { get; set; }

        [ForeignKey(nameof(IdLangue))]
        public virtual Langue Langue { get; set; }

        // Pays
        [Column("pys_idpays"), Required]
        public int IdPays { get; set; }

        [ForeignKey(nameof(IdPays))]
        public virtual Pays Pays { get; set; }

        // Pays favori
        [Column("utl_idpaysfavori")]
        public int? IdPaysFavori { get; set; }

        [ForeignKey(nameof(IdPaysFavori))]
        [OnDelete(DeleteBehavior.SetNull)]
        public virtual Pays? PaysFavori { get; set; }

        [InverseProperty(nameof(Commande.Utilisateur))]
        public virtual ICollection<Commande> Commandes { get; set; }

        [InverseProperty(nameof(VoteUtilisateur.Utilisateur))]
        public virtual ICollection<VoteUtilisateur> Votes { get; set; }
    }
}
