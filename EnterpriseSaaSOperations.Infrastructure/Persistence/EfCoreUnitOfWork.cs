using EnterpriseSaaSOperations.Application.Abstractions;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    internal sealed class EfCoreUnitOfWork(OperationsDbContext dbContext) : IUnitOfWork
    {
        private readonly OperationsDbContext _dbContext = dbContext;

        public async Task<IApplicationTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            return new EfCoreApplicationTransaction(transaction);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void ClearTrackedChanges()
        {
            _dbContext.ChangeTracker.Clear();
        }
    }
}
