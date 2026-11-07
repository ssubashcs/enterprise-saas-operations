using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Organizations
{
    public sealed class CreateOrganizationRequest
    {
        [Required]
        [StringLength(200)]
        public string OrganizationName { get; init; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string OwnerName { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(320)]
        public string Email { get; init; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string Password { get; init; } = string.Empty;
    }
}
