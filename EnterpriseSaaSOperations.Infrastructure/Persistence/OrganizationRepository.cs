using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    internal sealed class OrganizationRepository(OperationsDbContext dbContext) : IOrganizationRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        public void Add(Organization organization)
        {
            _dbContext.Organizations.Add(organization);
        }
    }
}
