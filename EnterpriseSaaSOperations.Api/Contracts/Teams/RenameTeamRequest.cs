using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Teams
{
    public sealed class RenameTeamRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; init; } = string.Empty;
    }
}
