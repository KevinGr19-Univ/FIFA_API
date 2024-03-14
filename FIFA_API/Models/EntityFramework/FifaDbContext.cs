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
        public virtual DbSet<PiedJoueur> PiedsJoueur { get; set; }
        public virtual DbSet<PosteJoueur> PostesJoueur { get; set; }
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
            mb.Entity<StockProduit>(entity =>
            {
                entity.HasKey(spr => new { spr.IdVCProduit, spr.IdTaille });
            });

            mb.Entity<VoteUtilisateur>(entity =>
            {
                entity.HasKey(vtl => new { vtl.IdUtilisateur, vtl.IdCouleur, vtl.IdTaille });
            });

            mb.Entity<Photo>(entity =>
            {
                ManyToMany<Photo, Album>(entity, "_albums", "Photos", "pht_id", "pub_id");
                ManyToMany<Photo, Article>(entity, "_articles", "Photos", "pht_id", "pub_id");
                ManyToMany<Photo, Blog>(entity, "_blogs", "Photos", "pht_id", "pub_id");
                ManyToMany<Photo, Joueur>(entity, "_joueurs", "Photos", "pht_id", "jou_id");
            });

            mb.Entity<Video>(entity =>
            {
                ManyToMany<Video, Article>(entity, "_articles", "Videos", "vid_id", "pub_id");
            });

            mb.Entity<Trophee>(entity =>
            {
                ManyToMany<Trophee, Joueur>(entity, "Joueurs", "Trophees", "tph_id", "jou_id");
            });

            mb.Entity<Produit>(entity =>
            {
                entity.HasMany<Produit>("Associes")
                    .WithMany("AssociesTo")
                    .UsingEntity("t_j_produitassocies_pas", j =>
                    {
                        j.Property<int>("AssociesId").HasColumnName("prd_id1");
                        j.Property<int>("AssociesToId").HasColumnName("prd_id2");
                        j.HasKey("AssociesId", "AssociesToId");
                    });

                ManyToMany<Produit, Couleur>(entity, "Couleurs", "Produits", "prd_id", "col_id");
                ManyToMany<Produit, TailleProduit>(entity, "Tailles", "Produits", "prd_id", "tpr_id");
            });

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

            OnModelCreatingPartial(mb);
        }

        partial void OnModelCreatingPartial(ModelBuilder mb);

        #region Utils
        private void GreaterThanZero(EntityTypeBuilder entity, params string[] columnNames) => GTZRequest(entity, ">", columnNames);
        private void GreaterOrEqualThanZero(EntityTypeBuilder entity, params string[] columnNames) => GTZRequest(entity, ">=", columnNames);

        private void ManyToMany<T,U>(EntityTypeBuilder<T> entity, string propT, string propU, string fkNameT, string fkNameU, string? joinTableName = null)
            where T : class where U : class
        {
            string typeT = typeof(T).Name.ToLower();
            string typeU = typeof(U).Name.ToLower();

            entity.HasMany<U>(propT)
                .WithMany(propU)
                .UsingEntity(joinTableName ?? $"t_j_{typeU}{typeT}_{typeU[..2]}{typeT[0]}", j =>
                {
                    j.Property($"{propT}Id").HasColumnName(fkNameT);
                    j.Property($"{propU}Id").HasColumnName(fkNameU);
                });
        }

        private void GTZRequest(EntityTypeBuilder entity, string symbol, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
                entity.HasCheckConstraint($"ck_{columnName}", $"{columnName} {symbol} 0");
        }
        #endregion
    }
}
