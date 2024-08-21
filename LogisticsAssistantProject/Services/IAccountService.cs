using LogisticsAssistantProject.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LogisticsAssistantProject.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<SignInResult> LoginUserAsync(LoginViewModel model);
        Task LogoutUserAsync();
    }
}
