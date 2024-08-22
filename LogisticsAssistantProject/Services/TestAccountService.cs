using LogisticsAssistantProject.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LogisticsAssistantProject.Services
{
    public class TestAccountService : IAccountService
    {
        public Task<SignInResult> LoginUserAsync(LoginViewModel model)
        {
            if(model.Username == "invalid")
            {
                return Task.FromResult(SignInResult.Failed);
            }

            return Task.FromResult(SignInResult.Success);
        }

        public Task LogoutUserAsync()
        {
            return Task.CompletedTask;
        }

        public Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            if(model.Username == "invalid")
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Failed to register user" }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
