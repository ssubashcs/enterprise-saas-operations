using EnterpriseSaaSOperations.Application;
using EnterpriseSaaSOperations.Application.OperationalRequests;
using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    internal sealed class OperationalRequestRepository(OperationsDbContext dbContext) : IOperationalRequestRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        public async Task AddAsync(OperationalRequest request, CancellationToken cancellationToken = default)
        {
            _dbContext.OperationalRequests.Add(request);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<OperationalRequest?> GetByIdAsync(int requestId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.OperationalRequests.AsNoTracking()
                                                 .SingleOrDefaultAsync(request =>
                                                                         request.Id == requestId &&
                                                                         request.OrganizationId == organizationId,
                                                                       cancellationToken);
        }

        public Task<OperationalRequest?> GetByIdForUpdateAsync(int requestId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.OperationalRequests.SingleOrDefaultAsync(request =>
                                                                         request.Id == requestId &&
                                                                         request.OrganizationId == organizationId,
                                                                       cancellationToken);
        }

        public async Task<IReadOnlyList<OperationalRequest>> GetPageAsync(int organizationId, int skip, int take, 
                                                                          OperationalRequestFilter filter, CancellationToken cancellationToken = default)
        {
            IQueryable<OperationalRequest> query = BuildFilteredQuery(organizationId, filter);

            // sorted by newest first
            return await query.OrderByDescending(request => request.CreatedAt)
                              .ThenByDescending(request => request.Id)
                              .Skip(skip)
                              .Take(take)
                              .ToListAsync(cancellationToken);
        }

        public Task<int> CountAsync(int organizationId, OperationalRequestFilter filter, CancellationToken cancellationToken = default)
        {
            return BuildFilteredQuery(organizationId, filter).CountAsync(cancellationToken);
        }

        private IQueryable<OperationalRequest> BuildFilteredQuery(int organizationId, OperationalRequestFilter filter)
        {
            IQueryable<OperationalRequest> query = _dbContext.OperationalRequests
                                                             .AsNoTracking()
                                                             .Where(request => request.OrganizationId == organizationId);

            // when a filter is null, its condition isn't added to the query.
            if (filter.Status is { } status)
            {
                query = query.Where(request => request.Status == status);
            }

            if (filter.Priority is { } priority)
            {
                query = query.Where(request => request.Priority == priority);
            }

            if (filter.TeamId is { } teamId)
            {
                query = query.Where(request => request.TeamId == teamId);
            }

            if (filter.AssigneeUserId is { } assigneeUserId)
            {
                query = query.Where(request => request.AssigneeUserId == assigneeUserId);
            }

            return query;
        }
    }
}
