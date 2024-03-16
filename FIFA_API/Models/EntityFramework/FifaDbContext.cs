using FIFA_API.Models.Annotations;
using FIFA_API.Models.Utils;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using System.Reflection;
using System.Xml.Linq;

namespace FIFA_API.Models.EntityFramework
{
    public partial class FifaDbContext : DbContext
    {
        static FifaDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CodeStatusCommande>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PiedJoueur>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PosteJoueur>();
        }

        public FifaDbContext()
        {
            
        }

        public FifaDbContext(DbContextOptions<FifaDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Adresse> Adresses { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<CategorieProduit> CategorieProduits { get; set; }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Commande> Commandes { get; set; }
        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<Couleur> Couleurs { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<FaqJoueur> FaqJoueurs { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Joueur> Joueurs { get; set; }
        public virtual DbSet<Langue> Langues { get; set; }
        public virtual DbSet<LigneCommande> LigneCommandes { get; set; }
        public virtual DbSet<Nation> Nations { get; set; }
        public virtual DbSet<Pays> Pays { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Produit> Produits { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<Statistiques> Statistiques { get; set; }
        public virtual DbSet<StatusCommande> StatusCommandes { get; set; }
        public virtual DbSet<StockProduit> StockProduits { get; set; }
        public virtual DbSet<TailleProduit> TailleProduits { get; set; }
        public virtual DbSet<Trophee> Trophees { get; set; }
        public virtual DbSet<TypeLivraison> TypeLivraisons { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }
        public virtual DbSet<VarianteCouleurProduit> VarianteCouleurProduits { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<VoteUtilisateur> VoteUtilisateurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Uid=postgres;Password=postgres;Database=SAE401");
            }
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.HasPostgresEnum<CodeStatusCommande>();
            mb.HasPostgresEnum<PiedJoueur>();
            mb.HasPostgresEnum<PosteJoueur>();

            DbContextUtils.AddComposedPrimaryKeys(mb);
            DbContextUtils.AddManyToManyRelations(mb);
            DbContextUtils.AddDeleteBehaviors(mb);
            DbContextUtils.RenameConstraintsAuto(mb);

            AddDatabaseCheckConstraints(mb);
             
            OnModelCreatingPartial(mb);
        }

        private void AddDatabaseCheckConstraints(ModelBuilder mb)
        {
            mb.Entity<Adresse>(entity =>
            {
                entity.Property(adr => adr.CodePostal).IsFixedLength();
                entity.HasCheckConstraint("ck_adr_codepostal", $"adr_codepostal ~ '{ModelUtils.REGEX_CODEPOSTAL}'");
            });

            mb.Entity<Utilisateur>(entity =>
            {
                entity.Property(utl => utl.MotDePasse).IsFixedLength();
                entity.HasCheckConstraint("ck_utl_telephone", $"utl_telephone ~ '{ModelUtils.REGEX_TELEPHONE}'");
            });

            mb.Entity<Couleur>(entity =>
            {
                entity.Property(col => col.CodeHexa).IsFixedLength();
                entity.HasCheckConstraint("ck_col_codehexa", $"col_codehexa ~ '{ModelUtils.REGEX_HEXACOLOR}'");
            });

            mb.Entity<Commande>(entity =>
            {
                GreaterThanZero(entity, "cmd_prixlivraison");
            });

            mb.Entity<LigneCommande>(entity =>
            {
                GreaterThanZero(entity, "lco_quantite");
                GreaterOrEqualThanZero(entity, "lco_prixunitaire");
            });

            mb.Entity<Joueur>(entity =>
            {
                GreaterThanZero(entity, "jou_poids", "jou_taille");
            });

            mb.Entity<Statistiques>(entity =>
            {
                GreaterThanZero(entity, "stt_matchsjoues", "stt_titularisations", "stt_minutesjouees", "stt_buts");
            });

            mb.Entity<StockProduit>(entity =>
            {
                GreaterThanZero(entity, "spr_stocks");
            });

            mb.Entity<TypeLivraison>(entity =>
            {
                GreaterThanZero(entity, "tli_prix");
            });

            mb.Entity<VarianteCouleurProduit>(entity =>
            {
                GreaterThanZero(entity, "vcp_prix");
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder mb);

        #region Utils
        private void GreaterThanZero(EntityTypeBuilder entity, params string[] columnNames) => GreaterThanZeroCheck(entity, ">", columnNames);
        private void GreaterOrEqualThanZero(EntityTypeBuilder entity, params string[] columnNames) => GreaterThanZeroCheck(entity, ">=", columnNames);

        private void GreaterThanZeroCheck(EntityTypeBuilder entity, string symbol, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
                entity.HasCheckConstraint($"ck_{entity.Metadata.GetTableName()}_{columnName}", $"{columnName} {symbol} 0");
        }
        #endregion
    }
}
