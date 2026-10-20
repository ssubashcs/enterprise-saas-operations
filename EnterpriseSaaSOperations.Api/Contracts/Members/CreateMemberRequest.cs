using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Members
{
    public sealed class CreateMemberRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; init; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(320)]
        public string Email { get; init; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string Password { get; init; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int? TeamId { get; init; }
    }
}
