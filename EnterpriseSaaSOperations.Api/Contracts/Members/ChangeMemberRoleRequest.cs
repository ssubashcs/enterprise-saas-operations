using EnterpriseSaaSOperations.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Contracts.Members
{
    public sealed class ChangeMemberRoleRequest
    {
        /// <summary>
        /// The role to assign to the target user. Only Member and Administrator are permitted through this operation.
        /// </summary>
        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole? Role { get; init; }
    }
}
