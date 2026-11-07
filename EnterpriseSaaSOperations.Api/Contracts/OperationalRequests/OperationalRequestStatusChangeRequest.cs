using System.ComponentModel.DataAnnotations;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed class OperationalRequestStatusChangeRequest
    {
        [Required]
        public RequestStatus? NewStatus { get; init; }
    }
}
