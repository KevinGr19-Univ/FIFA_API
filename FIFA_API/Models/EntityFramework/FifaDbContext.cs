using FIFA_API.Models.Annotations;
using FIFA_API.Models.Utils;
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
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RoleUtilisateur>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CodeStatusCommande>();
        }

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
        public virtual DbSet<ProduitProduit> ProduitAssocies { get; set; }
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
            mb.HasPostgresEnum<RoleUtilisateur>();
            mb.HasPostgresEnum<CodeStatusCommande>();

            AddComposedPrimaryKeys(mb);
            AddManyToManyRelations(mb);
            AddDatabaseCheckConstraints(mb);
            AddDeleteBehaviors(mb);
            RenameConstraintsAuto(mb);

            OnModelCreatingPartial(mb);
        }

        private void AddComposedPrimaryKeys(ModelBuilder mb)
        {
            foreach(var entity in mb.Model.GetEntityTypes())
            {
                if (entity.IsPropertyBag) continue;
                ComposedKeyAttribute? cKey = entity.ClrType.GetCustomAttribute<ComposedKeyAttribute>();
                if (cKey != null) mb.Entity(entity.ClrType).HasKey(cKey.keys);
            }
        }

        private void AddManyToManyRelations(ModelBuilder mb)
        {
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
                ManyToMany<Trophee, Joueur>(entity,
                    new()
                    {
                        PropertyName = "Joueurs",
                        ColumnName = "jou_id",
                        DeleteBehavior = DeleteBehavior.Restrict
                    },
                    new()
                    {
                        PropertyName = "Trophees",
                        ColumnName = "tph_id",
                        DeleteBehavior = DeleteBehavior.SetNull
                    });
            });

            mb.Entity<Produit>(entity =>
            {
                ManyToMany<Produit, Couleur>(entity, "Couleurs", "Produits", "prd_id", "col_id");
                ManyToMany<Produit, TailleProduit>(entity, "Tailles", "Produits", "prd_id", "tpr_id");
            });
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

        public const DeleteBehavior DEFAULT_DELETE = DeleteBehavior.Restrict;
        public const DeleteBehavior DEFAULT_DELETE_MANY_TO_MANY = DeleteBehavior.Cascade;

        private void AddDeleteBehaviors(ModelBuilder mb)
        {
            foreach(var fk in mb.Model.GetEntityTypes().SelectMany(e => e.GetDeclaredForeignKeys()))
            {
                if (fk.DeclaringEntityType.IsPropertyBag) continue; // Class-less -> No attribute

                DeleteBehavior? deleteBehavior = fk.GetNavigations()
                    .Select(n => n.PropertyInfo)
                    .Select(p => p.GetCustomAttribute<OnDeleteAttribute>())
                    .Where(a => a != null)
                    .FirstOrDefault()?.deleteBehavior;

                fk.DeleteBehavior = deleteBehavior ?? DEFAULT_DELETE;
            }
        }

        private void RenameConstraintsAuto(ModelBuilder mb)
        {
            foreach (var entity in mb.Model.GetEntityTypes())
            {
                string tableName = entity.GetTableName()!.Split("_")[2];

                foreach (var fk in entity.GetDeclaredForeignKeys())
                {
                    string fkName = GetConstraintName("FK", tableName, fk.Properties);
                    fk.SetConstraintName(fkName);
                }

                foreach (var ix in entity.GetIndexes())
                {
                    string ixName = GetConstraintName("IX", tableName, ix.Properties);
                    ix.SetDatabaseName(ixName);
                }
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder mb);

        #region Utils
        private void GreaterThanZero(EntityTypeBuilder entity, params string[] columnNames) => GTZRequest(entity, ">", columnNames);
        private void GreaterOrEqualThanZero(EntityTypeBuilder entity, params string[] columnNames) => GTZRequest(entity, ">=", columnNames);

        private void ManyToMany<T,U>(EntityTypeBuilder<T> entity, string propT, string propU, string fkNameT, string fkNameU, string? joinTableName = null)
            where T : class where U : class
        {
            ManyToMany(entity,
                new ManyToManyProperty<T>
                {
                    PropertyName = propT,
                    ColumnName = fkNameT
                },
                new ManyToManyProperty<U>
                {
                    PropertyName = propU,
                    ColumnName = fkNameU
                },
                joinTableName);
        }

        private void ManyToMany<T,U>(EntityTypeBuilder<T> entity, ManyToManyProperty<T> objT, ManyToManyProperty<U> objU, string? joinTableName = null)
            where T : class where U : class
        {
            entity.HasMany<U>(objT.PropertyName)
                .WithMany(objU.PropertyName)
                .UsingEntity(joinTableName ?? $"t_j_{objU.TypeName}{objT.TypeName}_{objU.TypeName[..2]}{objT.TypeName[0]}", j =>
                {
                    string tId = $"{objT.PropertyName}Id";
                    string uId = $"{objU.PropertyName}Id";

                    (objT.KeyType == null ? j.Property(tId) : j.Property(objT.KeyType, tId)).HasColumnName(objT.ColumnName);
                    (objU.KeyType == null ? j.Property(uId) : j.Property(objU.KeyType, uId)).HasColumnName(objU.ColumnName);

                    j.HasKey(tId, uId);

                    foreach (var fk in j.Metadata.GetDeclaredForeignKeys())
                    {
                        if (fk.Properties[0].GetColumnBaseName() == objT.ColumnName) fk.DeleteBehavior = objT.DeleteBehavior ?? DEFAULT_DELETE_MANY_TO_MANY;
                        else if (fk.Properties[0].GetColumnBaseName() == objU.ColumnName) fk.DeleteBehavior = objU.DeleteBehavior ?? DEFAULT_DELETE_MANY_TO_MANY;
                    }
                });
        }

        private void GTZRequest(EntityTypeBuilder entity, string symbol, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
                entity.HasCheckConstraint($"ck_{entity.Metadata.GetTableName()}_{columnName}", $"{columnName} {symbol} 0");
        }

        private string GetConstraintName(string prefix, string tableName, IReadOnlyList<IMutableProperty> properties)
            => $"{prefix}_{tableName}_{string.Join("_", properties.Select(p => p.GetColumnBaseName()))}";
        #endregion
    }
}
