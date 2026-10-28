namespace EnterpriseSaaSOperations.Application.Auditing
{
    /// <summary>
    /// Identifies the business operation recorded in an audit entry.
    /// </summary>
    public enum AuditAction
    {
        MemberRoleChanged,
        OperationalRequestAssigned, 
        OperationalRequestStatusChanged
    }
}
