using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public class OperationalRequestAssignmentRequest
    {
        [Range(1, int.MaxValue)]
        public int TeamId { get; init; }

        [Range(1, int.MaxValue)]
        public int AssigneeUserId { get; init; }
    }
}
