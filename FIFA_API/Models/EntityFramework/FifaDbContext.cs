using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.EntityFramework
{
    public partial class FifaDbContext : DbContext
    {
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
                optionsBuilder.UseNpgsql("Server:localhost;Port=5432;Uid=postgres;Password=postgres;Database=SAE401");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder builder);
    }
}
