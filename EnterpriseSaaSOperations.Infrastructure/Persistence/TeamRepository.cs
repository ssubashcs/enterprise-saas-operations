using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    /// <summary>
    /// Implements organization-scoped team persistence through EF Core.
    /// </summary>
    internal sealed class TeamRepository(OperationsDbContext dbContext) : ITeamRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;    

        public void Add(Team team)
        {
            _dbContext.Teams.Add(team);
        }

        public Task<Team?> GetByIdAsync(int teamId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Teams.AsNoTracking()
                                   .SingleOrDefaultAsync(team => team.Id == teamId && 
                                                                 team.OrganizationId == organizationId,
                                                         cancellationToken);
        }

        /// <inheritdoc />
        public Task<Team?> GetByIdForUpdateAsync(int teamId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Teams.SingleOrDefaultAsync(team => team.Id == teamId &&
                                                                 team.OrganizationId == organizationId,
                                                         cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<Team>> GetPageAsync(int organizationId, int skip, int take, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Teams.AsNoTracking()
                                         .Where(team => team.OrganizationId == organizationId)
                                         .OrderBy(team => team.Id)
                                         .Skip(skip)
                                         .Take(take)
                                         .ToListAsync(cancellationToken);
        }

        public Task<int> CountByOrganizationAsync(int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Teams.CountAsync(team => team.OrganizationId == organizationId, cancellationToken);
        }
    }
}
