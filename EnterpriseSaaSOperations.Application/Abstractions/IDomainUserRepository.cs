using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface IDomainUserRepository
    {
        /// <summary>
        /// Adds a domain user to the current unit of work.
        /// </summary>
        void Add(User user);

        Task<User?> GetByIdForUpdateAsync(int userId, int organizationId, CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(int userId, int organizationId, CancellationToken cancellationToken = default);
    }
}
