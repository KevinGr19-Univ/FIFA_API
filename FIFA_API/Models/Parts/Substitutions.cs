namespace FIFA_API.Models.EntityFramework
{
    public partial class Album
    {
        public override bool Equals(object? obj)
        {
            return obj is Album album &&
                   Id == album.Id &&
                   Titre == album.Titre &&
                   Resume == album.Resume &&
                   DatePublication == album.DatePublication &&
                   IdPhoto == album.IdPhoto &&
                   EqualityComparer<Photo?>.Default.Equals(Photo, album.Photo) &&
                   Visible == album.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Titre, Resume, DatePublication, IdPhoto, Photo, Visible);
        }
    }

    public partial class Article
    {
        public override bool Equals(object? obj)
        {
            return obj is Article article &&
                   Id == article.Id &&
                   Titre == article.Titre &&
                   Resume == article.Resume &&
                   DatePublication == article.DatePublication &&
                   IdPhoto == article.IdPhoto &&
                   EqualityComparer<Photo?>.Default.Equals(Photo, article.Photo) &&
                   EqualityComparer<ICollection<Joueur>>.Default.Equals(Joueurs, article.Joueurs) &&
                   Visible == article.Visible &&
                   Texte == article.Texte;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Titre);
            hash.Add(Resume);
            hash.Add(DatePublication);
            hash.Add(IdPhoto);
            hash.Add(Photo);
            hash.Add(Joueurs);
            hash.Add(Visible);
            hash.Add(Texte);
            return hash.ToHashCode();
        }
    }

    public partial class Blog
    {
        public override bool Equals(object? obj)
        {
            return obj is Blog blog &&
                   Id == blog.Id &&
                   Titre == blog.Titre &&
                   Resume == blog.Resume &&
                   DatePublication == blog.DatePublication &&
                   IdPhoto == blog.IdPhoto &&
                   EqualityComparer<Photo?>.Default.Equals(Photo, blog.Photo) &&
                   Visible == blog.Visible &&
                   Texte == blog.Texte;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Titre);
            hash.Add(Resume);
            hash.Add(DatePublication);
            hash.Add(IdPhoto);
            hash.Add(Photo);
            hash.Add(Visible);
            hash.Add(Texte);
            return hash.ToHashCode();
        }
    }

    public partial class CategorieProduit
    {
        public override bool Equals(object? obj)
        {
            return obj is CategorieProduit produit &&
                   Id == produit.Id &&
                   Nom == produit.Nom &&
                   IdCategorieProduitParent == produit.IdCategorieProduitParent &&
                   Visible == produit.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom, IdCategorieProduitParent, Visible);
        }
    }

    public partial class Club
    {
        public override bool Equals(object? obj)
        {
            return obj is Club club &&
                   Id == club.Id &&
                   Nom == club.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom);
        }
    }

    public partial class Commande
    {
        public override bool Equals(object? obj)
        {
            return obj is Commande commande &&
                   Id == commande.Id &&
                   IdTypeLivraison == commande.IdTypeLivraison &&
                   EqualityComparer<TypeLivraison>.Default.Equals(TypeLivraison, commande.TypeLivraison) &&
                   IdUtilisateur == commande.IdUtilisateur &&
                   EqualityComparer<Utilisateur?>.Default.Equals(Utilisateur, commande.Utilisateur) &&
                   VilleLivraison == commande.VilleLivraison &&
                   RueLivraison == commande.RueLivraison &&
                   CodePostalLivraison == commande.CodePostalLivraison &&
                   VilleFacturation == commande.VilleFacturation &&
                   RueFacturation == commande.RueFacturation &&
                   CodePostalFacturation == commande.CodePostalFacturation &&
                   PrixLivraison == commande.PrixLivraison &&
                   DateCommande == commande.DateCommande &&
                   DateExpedition == commande.DateExpedition &&
                   DateLivraison == commande.DateLivraison &&
                   UrlFacture == commande.UrlFacture &&
                   EqualityComparer<ICollection<LigneCommande>>.Default.Equals(Lignes, commande.Lignes) &&
                   EqualityComparer<ICollection<StatusCommande>>.Default.Equals(Status, commande.Status);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(IdTypeLivraison);
            hash.Add(TypeLivraison);
            hash.Add(IdUtilisateur);
            hash.Add(Utilisateur);
            hash.Add(VilleLivraison);
            hash.Add(RueLivraison);
            hash.Add(CodePostalLivraison);
            hash.Add(VilleFacturation);
            hash.Add(RueFacturation);
            hash.Add(CodePostalFacturation);
            hash.Add(PrixLivraison);
            hash.Add(DateCommande);
            hash.Add(DateExpedition);
            hash.Add(DateLivraison);
            hash.Add(UrlFacture);
            hash.Add(Lignes);
            hash.Add(Status);
            return hash.ToHashCode();
        }
    }

    public partial class CommentaireBlog
    {

    }

    public partial class Competition
    {
        public override bool Equals(object? obj)
        {
            return obj is Competition competition &&
                   Id == competition.Id &&
                   Nom == competition.Nom &&
                   Visible == competition.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom, Visible);
        }
    }

    public partial class Couleur
    {
        public override bool Equals(object? obj)
        {
            return obj is Couleur couleur &&
                   Id == couleur.Id &&
                   Nom == couleur.Nom &&
                   CodeHexa == couleur.CodeHexa &&
                   Visible == couleur.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom, CodeHexa, Visible);
        }
    }

    public partial class Document
    {
        public override bool Equals(object? obj)
        {
            return obj is Document document &&
                   Id == document.Id &&
                   Titre == document.Titre &&
                   Resume == document.Resume &&
                   DatePublication == document.DatePublication &&
                   IdPhoto == document.IdPhoto &&
                   EqualityComparer<Photo?>.Default.Equals(Photo, document.Photo) &&
                   Visible == document.Visible &&
                   UrlPdf == document.UrlPdf;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Titre);
            hash.Add(Resume);
            hash.Add(DatePublication);
            hash.Add(IdPhoto);
            hash.Add(Photo);
            hash.Add(Visible);
            hash.Add(UrlPdf);
            return hash.ToHashCode();
        }
    }

    public partial class FaqJoueur
    {

    }

    public partial class Genre
    {
        public override bool Equals(object? obj)
        {
            return obj is Genre genre &&
                   Id == genre.Id &&
                   Nom == genre.Nom &&
                   Visible == genre.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom, Visible);
        }
    }

    public partial class Joueur
    {
        public override bool Equals(object? obj)
        {
            return obj is Joueur joueur &&
                   Id == joueur.Id &&
                   Nom == joueur.Nom &&
                   Prenom == joueur.Prenom &&
                   DateNaissance == joueur.DateNaissance &&
                   LieuNaissance == joueur.LieuNaissance &&
                   Poids == joueur.Poids &&
                   Taille == joueur.Taille &&
                   Biographie == joueur.Biographie &&
                   ImageUrl == joueur.ImageUrl &&
                   EqualityComparer<Statistiques?>.Default.Equals(Stats, joueur.Stats) &&
                   IdClub == joueur.IdClub &&
                   EqualityComparer<Club>.Default.Equals(Club, joueur.Club) &&
                   IdPays == joueur.IdPays &&
                   EqualityComparer<Pays>.Default.Equals(Pays, joueur.Pays) &&
                   Pied == joueur.Pied &&
                   Poste == joueur.Poste;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Nom);
            hash.Add(Prenom);
            hash.Add(DateNaissance);
            hash.Add(LieuNaissance);
            hash.Add(Poids);
            hash.Add(Taille);
            hash.Add(Biographie);
            hash.Add(ImageUrl);
            hash.Add(Stats);
            hash.Add(IdClub);
            hash.Add(Club);
            hash.Add(IdPays);
            hash.Add(Pays);
            hash.Add(Pied);
            hash.Add(Poste);
            return hash.ToHashCode();
        }
    }

    public partial class Langue
    {
        public override bool Equals(object? obj)
        {
            return obj is Langue langue &&
                   Id == langue.Id &&
                   Nom == langue.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom);
        }
    }

    public partial class LigneCommande
    {
        public override bool Equals(object? obj)
        {
            return obj is LigneCommande commande &&
                   Id == commande.Id &&
                   Quantite == commande.Quantite &&
                   PrixUnitaire == commande.PrixUnitaire &&
                   IdVCProduit == commande.IdVCProduit &&
                   EqualityComparer<VarianteCouleurProduit>.Default.Equals(VCProduit, commande.VCProduit) &&
                   IdTaille == commande.IdTaille &&
                   EqualityComparer<TailleProduit>.Default.Equals(Taille, commande.Taille) &&
                   IdCommande == commande.IdCommande &&
                   EqualityComparer<Commande>.Default.Equals(Commande, commande.Commande);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Quantite);
            hash.Add(PrixUnitaire);
            hash.Add(IdVCProduit);
            hash.Add(VCProduit);
            hash.Add(IdTaille);
            hash.Add(Taille);
            hash.Add(IdCommande);
            hash.Add(Commande);
            return hash.ToHashCode();
        }
    }

    public partial class Nation
    {
        public override bool Equals(object? obj)
        {
            return obj is Nation nation &&
                   Id == nation.Id &&
                   Nom == nation.Nom &&
                   Visible == nation.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom, Visible);
        }
    }

    public partial class Pays
    {
        public override bool Equals(object? obj)
        {
            return obj is Pays pays &&
                   Id == pays.Id &&
                   Nom == pays.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom);
        }
    }

    public partial class Photo
    {

    }

    public partial class Produit
    {
        public override bool Equals(object? obj)
        {
            return obj is Produit produit &&
                   Id == produit.Id &&
                   Titre == produit.Titre &&
                   Description == produit.Description &&
                   IdCompetition == produit.IdCompetition &&
                   IdNation == produit.IdNation &&
                   IdGenre == produit.IdGenre &&
                   IdCategorieProduit == produit.IdCategorieProduit &&
                   EqualityComparer<Competition?>.Default.Equals(Competition, produit.Competition) &&
                   EqualityComparer<Nation?>.Default.Equals(Nation, produit.Nation) &&
                   EqualityComparer<Genre?>.Default.Equals(Genre, produit.Genre) &&
                   EqualityComparer<CategorieProduit>.Default.Equals(Categorie, produit.Categorie) &&
                   Visible == produit.Visible;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Titre);
            hash.Add(Description);
            hash.Add(IdCompetition);
            hash.Add(IdNation);
            hash.Add(IdGenre);
            hash.Add(IdCategorieProduit);
            hash.Add(Competition);
            hash.Add(Nation);
            hash.Add(Genre);
            hash.Add(Categorie);
            hash.Add(Visible);
            return hash.ToHashCode();
        }
    }

    public partial class Publication
    {
        public override bool Equals(object? obj)
        {
            return obj is Publication publication &&
                   Type == publication.Type &&
                   Id == publication.Id &&
                   Titre == publication.Titre &&
                   Resume == publication.Resume &&
                   DatePublication == publication.DatePublication &&
                   IdPhoto == publication.IdPhoto &&
                   EqualityComparer<Photo?>.Default.Equals(Photo, publication.Photo) &&
                   Visible == publication.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Id, Titre, Resume, DatePublication, IdPhoto, Photo, Visible);
        }
    }

    public partial class Statistiques
    {
        public override bool Equals(object? obj)
        {
            return obj is Statistiques statistiques &&
                   IdJoueur == statistiques.IdJoueur &&
                   MatchsJoues == statistiques.MatchsJoues &&
                   Titularisations == statistiques.Titularisations &&
                   MinutesJouees == statistiques.MinutesJouees &&
                   Buts == statistiques.Buts;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdJoueur, MatchsJoues, Titularisations, MinutesJouees, Buts);
        }
    }

    public partial class StatusCommande
    {

    }

    public partial class StockProduit
    {
        public override bool Equals(object? obj)
        {
            return obj is StockProduit produit &&
                   IdVCProduit == produit.IdVCProduit &&
                   IdTaille == produit.IdTaille &&
                   EqualityComparer<VarianteCouleurProduit>.Default.Equals(VCProduit, produit.VCProduit) &&
                   EqualityComparer<TailleProduit>.Default.Equals(Taille, produit.Taille) &&
                   Stocks == produit.Stocks;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdVCProduit, IdTaille, VCProduit, Taille, Stocks);
        }
    }

    public partial class TailleProduit
    {
        public override bool Equals(object? obj)
        {
            return obj is TailleProduit produit &&
                   Id == produit.Id &&
                   Nom == produit.Nom &&
                   Visible == produit.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom, Visible);
        }
    }

    public partial class ThemeVote
    {
        public override bool Equals(object? obj)
        {
            return obj is ThemeVote vote &&
                   Id == vote.Id &&
                   NomTheme == vote.NomTheme &&
                   Visible == vote.Visible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NomTheme, Visible);
        }
    }

    public partial class ThemeVoteJoueur
    {

    }

    public partial class Trophee
    {
        public override bool Equals(object? obj)
        {
            return obj is Trophee trophee &&
                   Id == trophee.Id &&
                   Nom == trophee.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom);
        }
    }

    public partial class TypeLivraison
    {
        public override bool Equals(object? obj)
        {
            return obj is TypeLivraison livraison &&
                   Id == livraison.Id &&
                   Nom == livraison.Nom &&
                   MaxBusinessDays == livraison.MaxBusinessDays &&
                   Prix == livraison.Prix;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nom, MaxBusinessDays, Prix);
        }
    }

    public partial class Utilisateur
    {
        public override bool Equals(object? obj)
        {
            return obj is Utilisateur utilisateur &&
                   Id == utilisateur.Id &&
                   Prenom == utilisateur.Prenom &&
                   Surnom == utilisateur.Surnom &&
                   Telephone == utilisateur.Telephone &&
                   Mail == utilisateur.Mail &&
                   StripeId == utilisateur.StripeId &&
                   RefreshToken == utilisateur.RefreshToken &&
                   DateNaissance == utilisateur.DateNaissance &&
                   HashMotDePasse == utilisateur.HashMotDePasse &&
                   DerniereConnexion == utilisateur.DerniereConnexion &&
                   DateVerificationEmail == utilisateur.DateVerificationEmail &&
                   DoubleAuthentification == utilisateur.DoubleAuthentification &&
                   Token2FA == utilisateur.Token2FA &&
                   DateVerif2FA == utilisateur.DateVerif2FA &&
                   Anonyme == utilisateur.Anonyme &&
                   VerifEmail == utilisateur.VerifEmail &&
                   Login2FA == utilisateur.Login2FA &&
                   Role == utilisateur.Role &&
                   IdLangue == utilisateur.IdLangue &&
                   EqualityComparer<Langue>.Default.Equals(Langue, utilisateur.Langue) &&
                   IdPays == utilisateur.IdPays &&
                   EqualityComparer<Pays>.Default.Equals(Pays, utilisateur.Pays) &&
                   IdPaysFavori == utilisateur.IdPaysFavori &&
                   EqualityComparer<Pays?>.Default.Equals(PaysFavori, utilisateur.PaysFavori);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Prenom);
            hash.Add(Surnom);
            hash.Add(Telephone);
            hash.Add(Mail);
            hash.Add(StripeId);
            hash.Add(RefreshToken);
            hash.Add(DateNaissance);
            hash.Add(HashMotDePasse);
            hash.Add(DerniereConnexion);
            hash.Add(DateVerificationEmail);
            hash.Add(DoubleAuthentification);
            hash.Add(Token2FA);
            hash.Add(DateVerif2FA);
            hash.Add(Anonyme);
            hash.Add(VerifEmail);
            hash.Add(Login2FA);
            hash.Add(Role);
            hash.Add(IdLangue);
            hash.Add(Langue);
            hash.Add(IdPays);
            hash.Add(Pays);
            hash.Add(IdPaysFavori);
            hash.Add(PaysFavori);
            return hash.ToHashCode();
        }
    }

    public partial class VarianteCouleurProduit
    {
        public override bool Equals(object? obj)
        {
            return obj is VarianteCouleurProduit produit &&
                   Id == produit.Id &&
                   IdProduit == produit.IdProduit &&
                   IdCouleur == produit.IdCouleur &&
                   Prix == produit.Prix &&
                   EqualityComparer<List<string>>.Default.Equals(ImageUrls, produit.ImageUrls) &&
                   EqualityComparer<Produit>.Default.Equals(Produit, produit.Produit) &&
                   EqualityComparer<Couleur>.Default.Equals(Couleur, produit.Couleur) &&
                   EqualityComparer<ICollection<StockProduit>>.Default.Equals(Stocks, produit.Stocks) &&
                   Visible == produit.Visible;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(IdProduit);
            hash.Add(IdCouleur);
            hash.Add(Prix);
            hash.Add(ImageUrls);
            hash.Add(Produit);
            hash.Add(Couleur);
            hash.Add(Stocks);
            hash.Add(Visible);
            return hash.ToHashCode();
        }
    }

    public partial class Video
    {

    }

    public partial class VoteUtilisateur
    {
        public override bool Equals(object? obj)
        {
            return obj is VoteUtilisateur utilisateur &&
                   IdUtilisateur == utilisateur.IdUtilisateur &&
                   IdTheme == utilisateur.IdTheme &&
                   IdJoueur1 == utilisateur.IdJoueur1 &&
                   IdJoueur2 == utilisateur.IdJoueur2 &&
                   IdJoueur3 == utilisateur.IdJoueur3 &&
                   EqualityComparer<Utilisateur>.Default.Equals(Utilisateur, utilisateur.Utilisateur) &&
                   EqualityComparer<ThemeVote>.Default.Equals(ThemeVote, utilisateur.ThemeVote) &&
                   EqualityComparer<Joueur>.Default.Equals(Joueur1, utilisateur.Joueur1) &&
                   EqualityComparer<Joueur>.Default.Equals(Joueur2, utilisateur.Joueur2) &&
                   EqualityComparer<Joueur>.Default.Equals(Joueur3, utilisateur.Joueur3);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(IdUtilisateur);
            hash.Add(IdTheme);
            hash.Add(IdJoueur1);
            hash.Add(IdJoueur2);
            hash.Add(IdJoueur3);
            hash.Add(Utilisateur);
            hash.Add(ThemeVote);
            hash.Add(Joueur1);
            hash.Add(Joueur2);
            hash.Add(Joueur3);
            return hash.ToHashCode();
        }
    }

    public partial class AuthEmailVerif
    {

    }

    public partial class AuthPasswordReset
    {

    }

    public partial class Auth2FALogin
    {

    }

}
