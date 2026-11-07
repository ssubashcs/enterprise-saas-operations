using Microsoft.EntityFrameworkCore;

using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Repositories
{
    internal sealed class DomainUserRepository(OperationsDbContext dbContext) : IDomainUserRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        public void Add(User user)
        {
            _dbContext.DomainUsers.Add(user);
        }

        public Task<User?> GetByIdAsync(int userId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.DomainUsers.AsNoTracking()
                                         .SingleOrDefaultAsync(user => user.Id == userId &&
                                                                       user.OrganizationId == organizationId,
                                                               cancellationToken);
        }

        public Task<User?> GetByIdForUpdateAsync(int userId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.DomainUsers.SingleOrDefaultAsync(user => user.Id == userId &&
                                                                       user.OrganizationId == organizationId,
                                                               cancellationToken);
        }
    }
}
