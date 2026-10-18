using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Teams
{
    public sealed class CreateTeamRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; init; } = string.Empty;
    }
}
