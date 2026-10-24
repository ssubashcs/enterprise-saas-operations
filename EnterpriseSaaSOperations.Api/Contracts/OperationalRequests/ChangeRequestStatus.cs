using EnterpriseSaaSOperations.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed class ChangeRequestStatusRequest
    {
        [Required]
        public RequestStatus? NewStatus { get; init; }
    }
}
