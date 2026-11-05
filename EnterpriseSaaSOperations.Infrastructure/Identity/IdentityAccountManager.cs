using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using Microsoft.AspNetCore.Identity;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    internal sealed class IdentityAccountManager(UserManager<ApplicationIdentityUser> userManager) : IIdentityAccountManager
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager = userManager;

        public async Task<IdentityAccountCreationResult> CreateAsync(int domainUserId, string email, string password)
        {
            var emailAddress = email.Trim();

            ApplicationIdentityUser identityUser = new()
            {
                UserName = emailAddress,
                Email = emailAddress,
                DomainUserId = domainUserId
            };

            var result = await _userManager.CreateAsync(identityUser, password);

            return new IdentityAccountCreationResult(result.Succeeded, result.Errors.Select(error => error.Description).ToArray());
        }
    }
}
