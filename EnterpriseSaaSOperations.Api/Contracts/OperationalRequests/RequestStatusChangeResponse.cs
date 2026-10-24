using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed record RequestStatusChangeResponse(int RequestId,
                                                     RequestStatus PreviousStatus,
                                                     RequestStatus CurrentStatus,
                                                     DateTimeOffset UpdatedAt);
}
