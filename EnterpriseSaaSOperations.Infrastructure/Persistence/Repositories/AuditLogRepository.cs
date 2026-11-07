using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Auditing;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Repositories
{
    internal sealed class AuditLogRepository(OperationsDbContext dbContext) : IAuditLogRepository
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        public void Add(AuditEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);

            _dbContext.AuditEntries.Add(entry);
        }
    }
}
