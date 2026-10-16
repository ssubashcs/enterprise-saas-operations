using EnterpriseSaaSOperations.Application.Identity;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface ICurrentUserContextService
    {
        /// <summary>
        /// Resolves an authenticated Identity account to its current domain user.
        /// </summary>
        Task<CurrentUserAccessContext?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default);
    }
}
