using System.ComponentModel.DataAnnotations;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Api.Contracts.Users
{
    public sealed class ChangeUserRoleRequest
    {
        /// <summary>
        /// The role to assign to the target user. Only Member and Administrator are permitted through this operation.
        /// </summary>
        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole? Role { get; init; }
    }
}
