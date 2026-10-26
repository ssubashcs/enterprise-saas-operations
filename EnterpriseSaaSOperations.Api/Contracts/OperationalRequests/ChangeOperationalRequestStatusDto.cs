using EnterpriseSaaSOperations.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed class ChangeOperationalRequestStatusDto
    {
        [Required]
        public RequestStatus? NewStatus { get; init; }
    }
}
