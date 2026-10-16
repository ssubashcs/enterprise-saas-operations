using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    internal sealed class DomainUserRepository(OperationsDbContext dbContext) : IDomainUserRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        public void Add(User user)
        {
            _dbContext.DomainUsers.Add(user);
        }
    }
}
