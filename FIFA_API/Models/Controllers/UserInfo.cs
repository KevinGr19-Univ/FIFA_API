using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Models.Controllers
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string? Prenom { get; set; }
        public string? Surnom { get; set; }
        public DateTime? DateNaissance { get; set; }

        public string Mail { get; set; }
        public bool VerifEmail { get; set; }

        public string? Telephone { get; set; }
        public bool DoubleAuthentification { get; set; }
        public bool Login2FA { get; set; }

        public bool Anonyme { get; set; }

        public int IdLangue { get; set; }
        public int IdPays { get; set; }
        public int? IdPaysFavori { get; set; }

        public Utilisateur UpdateUser(Utilisateur user)
        {
            user.Prenom = Prenom;
            user.Surnom = Surnom;
            user.Mail = Mail;
            user.Telephone = Telephone;
            user.DateNaissance = DateNaissance;
            user.IdLangue = IdLangue;
            user.IdPays = IdPays;
            user.IdPaysFavori = IdPaysFavori;
            user.DoubleAuthentification = DoubleAuthentification;
            return user;
        }

        public static UserInfo FromUser(Utilisateur user)
        {
            return new()
            {
                Id = user.Id,
                Prenom = user.Prenom,
                Surnom = user.Surnom,
                Mail = user.Mail,
                Telephone = user.Telephone,
                DateNaissance = user.DateNaissance,
                IdLangue = user.IdLangue,
                IdPays = user.IdPays,
                IdPaysFavori = user.IdPaysFavori,
                VerifEmail = user.VerifEmail,
                Anonyme = user.Anonyme,
                DoubleAuthentification = user.DoubleAuthentification,
                Login2FA = user.Login2FA
            };
        }
    }
}
