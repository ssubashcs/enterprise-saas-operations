using Microsoft.AspNetCore.Identity;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    public sealed class AuthenticationService
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationIdentityUser> userManager, SignInManager<ApplicationIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<VerifiedCredentials?> VerifyCredentialsAsync(string email, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            string emailAddress = email.Trim();

            var identityUser = await _userManager.FindByEmailAsync(emailAddress);

            if (identityUser is null) return null;

            var signInResult = await _signInManager.CheckPasswordSignInAsync(identityUser, password, lockoutOnFailure: true);

            if (!signInResult.Succeeded) return null;

            return new VerifiedCredentials(identityUser.Id);
        }
    }
}
