using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.OperationalRequests
{
    /// <summary>
    /// Coordinates operational-request creation and retrieval.
    /// </summary>
    public sealed class OperationalRequestService(IOperationalRequestRepository repository)
    {
        private readonly IOperationalRequestRepository _repository = repository;

        /// <summary>
        /// Creates a request for the authenticated user's organization.
        /// </summary>
        public async Task<OperationalRequest> CreateAsync(CurrentUserAccessContext currentUser, string title, 
                                                          string description, RequestPriority priority,
                                                          CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            OperationalRequest request = new(currentUser.OrganizationId, currentUser.UserId, title,
                                             description, priority);

            await _repository.AddAsync(request, cancellationToken);

            return request;
        }

        /// <summary>
        /// Retrieves an operational request only if it belongs to the specified organization.
        /// </summary>
        public Task<OperationalRequest?> GetByIdAsync(int requestId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _repository.GetByIdAsync(requestId, organizationId, cancellationToken);
        }
    }
}
