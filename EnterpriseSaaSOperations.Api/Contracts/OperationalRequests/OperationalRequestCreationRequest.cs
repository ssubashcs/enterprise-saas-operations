using EnterpriseSaaSOperations.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed class OperationalRequestCreationRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; init; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; init; } = string.Empty;

        [Required]
        [EnumDataType(typeof(RequestPriority))]
        public RequestPriority? Priority { get; init; }
    }
}
