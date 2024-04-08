using FIFA_API.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FIFA_API.Services
{
    public class CustomAuthorizationService : ICustomAuthorizationService
    {
        private IAuthorizationService _authService;

        public CustomAuthorizationService(IAuthorizationService authService)
        {
            _authService = authService;
        }

        public async Task<bool> MatchPolicyAsync(ClaimsPrincipal user, string policy)
        {
            return (await _authService.AuthorizeAsync(user, policy)).Succeeded;
        }
    }
}
