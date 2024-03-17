using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Models
{
    public static class Policies
    {
        public const string Admin = "Admin";
        public const string ServiceExpedition = "ServiceExpedition";
        public const string ServiceCommande = "ServiceCommande";
        public const string DirecteurVente = "DirecteurVente";
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
