using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.OperationalRequests
{
    public sealed record RequestStatusChangeResult(bool Success,
                                                   int? RequestId,
                                                   RequestStatus? PreviousStatus,
                                                   RequestStatus? Status,
                                                   DateTimeOffset? UpdatedAt,
                                                   RequestStatusChangeFailure? Failure);
}
