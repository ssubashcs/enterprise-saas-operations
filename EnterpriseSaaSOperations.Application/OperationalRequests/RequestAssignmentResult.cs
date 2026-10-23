using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.OperationalRequests
{
    public sealed record RequestAssignmentResult(bool Success,
                                                 int? RequestId,
                                                 int? TeamId,
                                                 int? AssigneeUserId,
                                                 RequestStatus? Status,
                                                 RequestAssignmentFailure? Failure);
}
