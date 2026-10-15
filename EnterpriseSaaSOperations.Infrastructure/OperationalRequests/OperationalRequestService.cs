using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;
using EnterpriseSaaSOperations.Infrastructure.Identity;
using EnterpriseSaaSOperations.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.OperationalRequests
{
    /// <summary>
    /// Coordinates the persistence of operational-request business entities.
    /// </summary>
    public sealed class OperationalRequestService(OperationsDbContext dbContext)
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        /// <summary>
        /// Creates and persists an operational request for the specified user's organization.
        /// </summary>
        public async Task<OperationalRequest> CreateAsync(CurrentUserAccessContext currentUser, string title, 
                                                          string description, RequestPriority priority,
                                                          CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            OperationalRequest request = new(currentUser.OrganizationId, currentUser.UserId, title,
                                             description, priority);

            _dbContext.OperationalRequests.Add(request);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return request;
        }

        /// <summary>
        /// Retrieves an operational request only if it belongs to the specified organization.
        /// </summary>
        public async Task<OperationalRequest?> GetByIdAsync(int requestId, int organizationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.OperationalRequests.AsNoTracking()
                                                       .SingleOrDefaultAsync(request =>
                                                                                request.Id == requestId &&
                                                                                request.OrganizationId == organizationId,
                                                                             cancellationToken);
        }
    }
}
