using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.OperationalRequests
{
    /// <summary>
    /// Contains optional filters for querying operational requests.
    /// </summary>
    public sealed record OperationalRequestFilter(RequestStatus? Status = null,
                                                  RequestPriority? Priority = null,
                                                  int? TeamId = null,
                                                  int? AssigneeUserId = null);
}
