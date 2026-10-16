using EnterpriseSaaSOperations.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    internal sealed class EfCoreApplicationTransaction(IDbContextTransaction transaction) : IApplicationTransaction
    {
        private readonly IDbContextTransaction _transaction = transaction;
        private bool _completed;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
            _completed = true;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
            _completed = true;
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!_completed)
                {
                    await _transaction.RollbackAsync(CancellationToken.None);
                }
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }
    }
}
