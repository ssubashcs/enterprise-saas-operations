namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface IApplicationTransaction : IAsyncDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
