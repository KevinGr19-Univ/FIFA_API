using FIFA_API.Models.Controllers;

namespace FIFA_API.Contracts
{
    public interface IPasswordResetService
    {
        Task SendPasswordResetCodeAsync(string mail);

        Task<bool> ChangePasswordAsync(ChangePasswordRequest request, string code);
    }
}
