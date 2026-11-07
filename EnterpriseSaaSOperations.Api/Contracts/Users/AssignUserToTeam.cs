using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Users
{
    public sealed class AssignUserToTeam
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? TeamId { get; init; }
    }
}
