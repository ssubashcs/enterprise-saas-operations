using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Application
{
    public interface IOperationalRequestRepository
    {
        /// <summary>
        /// Persists a newly created operational request.
        /// </summary>
        Task AddAsync(OperationalRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a request only when it belongs to the specified organization.
        /// </summary>
        Task<OperationalRequest?> GetByIdAsync(int requestId, int organizationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a tracked operational request for modification, provided it belongs to the specified organization.
        /// </summary>
        Task<OperationalRequest?> GetByIdForUpdateAsync(int requestId, int organizationId, CancellationToken cancellationToken = default);
    }
}
