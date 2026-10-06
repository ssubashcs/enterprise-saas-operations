namespace EnterpriseSaaSOperations.Domain.Enums
{
    public enum RequestStatus
    {
        Created,
        Assigned,
        InProgress,
        Resolved,
        Closed
    }

    public enum RequestPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
