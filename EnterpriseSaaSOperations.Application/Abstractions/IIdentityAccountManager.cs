using EnterpriseSaaSOperations.Application.Identity;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface IIdentityAccountManager
    {
        Task<IdentityAccountCreationResult> CreateAsync(int domainUserId, string email, string password);
    }
}
