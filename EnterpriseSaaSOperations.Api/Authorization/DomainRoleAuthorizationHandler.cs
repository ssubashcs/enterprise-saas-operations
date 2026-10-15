using EnterpriseSaaSOperations.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseSaaSOperations.Api.Authorization
{
    /// <summary>
    /// Checks the current domain user's role against a <see cref="DomainRoleRequirement"/>.
    /// </summary>
    internal class DomainRoleAuthorizationHandler : AuthorizationHandler<DomainRoleRequirement>
    {
        private readonly CurrentUserContextService _currentUserContextService;

        public DomainRoleAuthorizationHandler(CurrentUserContextService currentUserContextService)
        {
            _currentUserContextService = currentUserContextService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DomainRoleRequirement requirement)
        {
            // The authenticated JWT identifies the Identity account through "sub".
            var identityUserId = context.User.FindFirst("sub")?.Value;

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
