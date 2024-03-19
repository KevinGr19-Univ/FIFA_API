using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Models.Controllers
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string? Prenom { get; set; }
        public string? Surnom { get; set; }
        public string Mail { get; set; }
        public string? Telephone { get; set; }
        public DateTime? DateNaissance { get; set; }
        public int IdLangue { get; set; }
        public Langue? Langue { get; set; }
        public int IdPays { get; set; }
        public Pays? Pays { get; set; }
        public int? IdPaysFavori { get; set; }
        public Pays? PaysFavori { get; set; }

        public Utilisateur UpdateUser(Utilisateur user)
        {
            user.Id = Id;
            user.Prenom = Prenom;
            user.Surnom = Surnom;
            user.Mail = Mail;
            user.Telephone = Telephone;
            user.DateNaissance = DateNaissance;
            user.IdLangue = IdLangue;
            user.IdPays = IdPays;
            user.IdPaysFavori = IdPaysFavori;
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
                Langue = user.Langue,
                IdPays = user.IdPays,
                Pays = user.PaysFavori,
                IdPaysFavori = user.IdPaysFavori,
                PaysFavori = user.PaysFavori
            };
        }
    }
}
