using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Members
{
    public sealed class AssignMemberToTeamRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? TeamId { get; init; }
    }
}
