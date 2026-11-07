using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed record OperationalRequestAssignmentResponse(int RequestId,
                                                              int TeamId,
                                                              int AssigneeUserId,
                                                              RequestStatus Status);
}
