using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Models
{
    /// <summary>
    /// Utilitaires pour gérer les permissions des utilisateurs de l'API.
    /// </summary>
    public static class Policies
    {
        /// <summary>
        /// Administrateur.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Membre du service d'expédition des commandes.
        /// </summary>
        public const string ServiceExpedition = "ServiceExpedition";

        /// <summary>
        /// Membre du service de gestion des commandes.
        /// </summary>
        public const string ServiceCommande = "ServiceCommande";

        /// <summary>
        /// Directeur des ventes et produits.
        /// </summary>
        public const string DirecteurVente = "DirecteurVente";

        /// <summary>
        /// Utilisateur authentifié.
        /// </summary>
        public const string User = "User";

        public static AuthorizationPolicy AdminPolicy()
        {
            return WithRoles(Admin).Build();
        }

        public static AuthorizationPolicy ServiceExpeditionPolicy()
        {
            return WithRoles(Admin, ServiceExpedition).Build();
        }

        public static AuthorizationPolicy ServiceCommandePolicy()
        {
            return WithRoles(Admin, ServiceCommande).Build();
        }

        public static AuthorizationPolicy DirecteurVentePolicy()
        {
            return WithRoles(Admin, DirecteurVente).Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        }

        private static AuthorizationPolicyBuilder WithRoles(params string[] roles)
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(roles);
        }
    }
}
