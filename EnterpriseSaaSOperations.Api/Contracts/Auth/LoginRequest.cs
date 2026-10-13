using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Auth
{
    public sealed class LoginRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(320)]
        public string Email { get; init; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string Password { get; init; } = string.Empty;
    }
}
