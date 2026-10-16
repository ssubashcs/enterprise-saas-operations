using EnterpriseSaaSOperations.Application;
using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    internal sealed class OperationalRequestRepository(OperationsDbContext dbContext) : IOperationalRequestRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        /// <inheritdoc/>
        public async Task AddAsync(OperationalRequest request, CancellationToken cancellationToken = default)
        {
            _dbContext.OperationalRequests.Add(request);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<OperationalRequest?> GetByIdAsync(int requestId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.OperationalRequests.AsNoTracking()
                                                 .SingleOrDefaultAsync(request =>
                                                                        request.Id == requestId &&
                                                                        request.OrganizationId == organizationId,
                                                                       cancellationToken);
        }
    }
}
