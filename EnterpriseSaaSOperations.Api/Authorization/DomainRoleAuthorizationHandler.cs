using EnterpriseSaaSOperations.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace EnterpriseSaaSOperations.Api.Authorization
{
    /// <summary>
    /// Checks the current domain user's role against a <see cref="DomainRoleRequirement"/>.
    /// </summary>
    internal class DomainRoleAuthorizationHandler : AuthorizationHandler<DomainRoleRequirement>
    {
        private readonly ICurrentUserContextService _currentUserContextService;

        public DomainRoleAuthorizationHandler(ICurrentUserContextService currentUserContextService)
        {
            _currentUserContextService = currentUserContextService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DomainRoleRequirement requirement)
        {
            // The JWT subject claim identifies the authenticated Identity account.
            string? identityUserId = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId)) return;

            var cancellationToken = context.Resource is HttpContext httpContext ? httpContext.RequestAborted : CancellationToken.None;

            var currentUser = await _currentUserContextService.GetByIdentityUserIdAsync(identityUserId, cancellationToken);

            if (currentUser is not null && requirement.AllowedRoles.Contains(currentUser.Role))
            {
                context.Succeed(requirement);
            }
        }
    }
}
