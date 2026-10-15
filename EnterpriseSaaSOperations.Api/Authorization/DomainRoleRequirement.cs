using EnterpriseSaaSOperations.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseSaaSOperations.Api.Authorization
{
    /// <summary>
    /// Describes the condition; which domain roles satisfy an authorization requirement.
    /// </summary>
    internal sealed class DomainRoleRequirement : IAuthorizationRequirement 
    {
        internal DomainRoleRequirement(params UserRole[] allowedRoles)
        {
            ArgumentNullException.ThrowIfNull(allowedRoles);

            if (allowedRoles.Length == 0)
            {
                throw new ArgumentException("At least one allowed role must be provided.", nameof(allowedRoles));
            }

            AllowedRoles = allowedRoles.ToHashSet();
        }

        internal IReadOnlySet<UserRole> AllowedRoles { get; }
    }
}
