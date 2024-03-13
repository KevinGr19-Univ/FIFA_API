using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIFA_API.Models.EntityFramework
{
    public partial class FifaDbContext : DbContext
    {
        public FifaDbContext() { }
        public FifaDbContext(DbContextOptions<FifaDbContext> options) : base(options) { }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockProduit>(entity =>
            {
                entity.HasKey(spr => new { spr.IdVCProduit, spr.IdTaille });
            });

            modelBuilder.Entity<VoteUtilisateur>(entity =>
            {
                entity.HasKey(vtl => new { vtl.IdUtilisateur, vtl.IdCouleur, vtl.IdTaille });
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasMany<Album>("_albums")
                    .WithMany(alb => alb.Photos)
                    .UsingEntity("t_j_albumphoto_alp");

                entity.HasMany<Article>("_articles")
                    .WithMany(art => art.Photos)
                    .UsingEntity("t_j_articlephoto_arp");

                entity.HasMany<Blog>("_blogs")
                    .WithMany(blg => blg.Photos)
                    .UsingEntity("t_j_blogphoto_blp");

                entity.HasMany<Joueur>("_joueurs")
                    .WithMany(jou => jou.Photos)
                    .UsingEntity("t_j_joueurphoto_jop");
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasMany<Article>("_articles")
                    .WithMany(art => art.Videos)
                    .UsingEntity("t_j_articlevideo_arv");
            });

            modelBuilder.Entity<Adresse>(entity =>
            {
                entity.Property(adr => adr.CodePostal).IsFixedLength();

                entity.HasCheckConstraint("ck_adr_codepostal", $"adr_codepostal ~ '{ModelUtils.REGEX_CODEPOSTAL}'");
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.Property(utl => utl.MotDePasse).IsFixedLength();

                entity.HasCheckConstraint("ck_utl_telephone", $"utl_telephone ~ '{ModelUtils.REGEX_TELEPHONE}'");
            });

            modelBuilder.Entity<Couleur>(entity =>
            {
                entity.Property(col => col.CodeHexa).IsFixedLength();

                entity.HasCheckConstraint("ck_col_codehexa", $"col_codehexa ~ '{ModelUtils.REGEX_HEXACOLOR}'");
            });

            modelBuilder.Entity<Commande>(entity =>
            {
                GreaterThanZero(entity, "cmd_prixlivraison");
            });

            modelBuilder.Entity<LigneCommande>(entity =>
            {
                GreaterThanZero(entity, "lco_quantite");
                GreaterOrEqualThanZero(entity, "lco_prixunitaire");
            });

            modelBuilder.Entity<Joueur>(entity =>
            {
                GreaterThanZero(entity, "jou_poids", "jou_taille");
            });

            modelBuilder.Entity<Statistiques>(entity =>
            {
                GreaterThanZero(entity, "stt_matchsjoues", "stt_titularisations", "stt_minutesjouees", "stt_buts");
            });

            modelBuilder.Entity<StockProduit>(entity =>
            {
                GreaterThanZero(entity, "spr_stocks");
            });

            modelBuilder.Entity<TypeLivraison>(entity =>
            {
                GreaterThanZero(entity, "tli_prix");
            });

            modelBuilder.Entity<VarianteCouleurProduit>(entity =>
            {
                GreaterThanZero(entity, "vcp_prix");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder builder);

        #region Utils
        private void GreaterThanZero(EntityTypeBuilder entity, params string[] columnNames) => GTZRequest(entity, ">", columnNames);
        private void GreaterOrEqualThanZero(EntityTypeBuilder entity, params string[] columnNames) => GTZRequest(entity, ">=", columnNames);

        private void GTZRequest(EntityTypeBuilder entity, string symbol, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
                entity.HasCheckConstraint($"ck_{columnName}", $"{columnName} {symbol} 0");
        }
        #endregion
    }
}
