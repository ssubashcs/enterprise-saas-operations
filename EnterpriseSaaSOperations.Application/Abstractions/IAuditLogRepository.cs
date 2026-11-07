using EnterpriseSaaSOperations.Application.Auditing;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    /// <summary>
    /// Provides persistence operations for business audit entries.
    /// </summary>
    public interface IAuditLogRepository
    {
        void Add(AuditEntry entry);
    }
}
