using FIFA_API.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace FIFA_API.Models.EntityFramework
{
    public partial class Utilisateur
    {
        /// <summary>
        /// Anonymise les informations d'un utilisateur.
        /// </summary>
        public void Anonymize()
        {
            if (Anonyme) return;

            string randomString(int length)
            {
                const string CHARS = "abcdefghijklmnopqrstuvwxyz";
                var rng = new Random((int)(DateTime.UtcNow.Ticks * 2045612) + 784810);
                var sb = new StringBuilder();

                for (int i = 0; i < length; i++) sb.Append(CHARS[rng.Next(CHARS.Length)]);
                return sb.ToString();
            }

            Prenom = null;
            Surnom = null;
            DateNaissance = null;
            Telephone = null;
            StripeId = null;
            RefreshToken = null;
            DateVerificationEmail = null;

            Mail = $"{randomString(20)}@{randomString(20)}.com";

            Anonyme = true;
        }
    }
}
