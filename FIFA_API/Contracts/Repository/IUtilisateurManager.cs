using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Contracts.Repository
{
    /// <summary>
    /// Contract used to manage instances of <see cref="Utilisateur"/>.
    /// </summary>
    public interface IUtilisateurManager : IRepository<Utilisateur>, IGetById<int, Utilisateur>
    {
        /// <summary>
        /// Gets a <see cref="Utilisateur"/> from its email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The <see cref="Utilisateur"/> with the matching email, <see langword="null"/> otherwise.</returns>
        Task<Utilisateur?> GetByEmailAsync(string email);

        /// <summary>
        /// Checks whether or not an email is already taken.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns><see langword="true"/> if the email is already taken, <see langword="false"/> otherwise.</returns>
        Task<bool> IsEmailTaken(string email);
    }
}
