using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    internal sealed class DomainUserRepository(OperationsDbContext dbContext) : IDomainUserRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        public void Add(User user)
        {
            _dbContext.DomainUsers.Add(user);
        }

        public Task<User?> GetByIdForUpdateAsync(int userId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _dbContext.DomainUsers.SingleOrDefaultAsync(user => user.Id == userId &&
                                                                       user.OrganizationId == organizationId,
                                                               cancellationToken);
        }
    }
}
