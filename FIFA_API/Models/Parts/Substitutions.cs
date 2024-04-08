namespace FIFA_API.Models.EntityFramework
{
    public partial class Album
    {

    }

    public partial class Article
    {

    }

    public partial class Blog
    {

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

    }

    public partial class Couleur
    {

    }

    public partial class Document
    {

    }

    public partial class FaqJoueur
    {

    }

    public partial class Genre
    {

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

    }

    public partial class Nation
    {

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

    }

    public partial class Publication
    {

    }

    public partial class Statistiques
    {

    }

    public partial class StatusCommande
    {

    }

    public partial class StockProduit
    {

    }

    public partial class TailleProduit
    {

    }

    public partial class ThemeVote
    {

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

    }

    public partial class VarianteCouleurProduit
    {

    }

    public partial class Video
    {

    }

    public partial class VoteUtilisateur
    {

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
