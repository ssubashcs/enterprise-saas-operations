namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begins a transaction covering the current unit of work.
        /// </summary>
        Task<IApplicationTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Discards tracked entity state after a rolled-back operation.
        /// </summary>
        void ClearTrackedChanges();
    }
}
