using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using EnterpriseSaaSOperations.Api.Authorization;
using EnterpriseSaaSOperations.Api.Contracts.Users;
using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Application.Members;
using EnterpriseSaaSOperations.Application.Teams;
using EnterpriseSaaSOperations.Application.Users;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ICurrentUserContextService _currentUserContextService;
        private readonly MemberProvisioningService _memberProvisioningService;
        private readonly UserRoleManagementService _memberRoleManagementService;
        private readonly TeamMembershipService _teamMembershipService;

        public UsersController(ICurrentUserContextService currentUserContextService, MemberProvisioningService memberProvisioningService,
                               UserRoleManagementService memberRoleManagementService, TeamMembershipService teamMembershipService)
        {
            _currentUserContextService = currentUserContextService;
            _memberProvisioningService = memberProvisioningService;
            _memberRoleManagementService = memberRoleManagementService;
            _teamMembershipService = teamMembershipService;
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.AdministratorOrOwner)]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var result = await _memberProvisioningService.CreateAsync(currentUser, request.Name, request.Email, request.Password,
                                                                      request.TeamId, cancellationToken);

            if (!result.Succeeded)
            {
                return result.Failure switch
                {
                    MemberProvisioningFailure.TeamNotFound =>
                        NotFound(new ProblemDetails
                        {
                            Status = StatusCodes.Status404NotFound,
                            Title = "Team not found.",
                            Detail = "The selected team was not found in your organization."
                        }),

                    MemberProvisioningFailure.IdentityAccountCreationFailed =>
                        BadRequest(new ValidationProblemDetails(
                            new Dictionary<string, string[]>
                            {
                                ["account"] =
                                [
                                    "The member account could not be created. " +
                                    "Check the email address and password requirements."
                                ]
                            })
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Member provisioning failed."
                        }),

                    _ => throw new InvalidOperationException($"Unexpected member-provisioning failure: {result.Failure}.")
                };
            }

            if (result.MemberUserId is not int memberUserId)
            {
                throw new InvalidOperationException("Successful provisioning must return the member's ID.");
            }

            var response = new CreateUserResponse(memberUserId, currentUser.OrganizationId, result.TeamId,
                                                          request.Name.Trim(), request.Email.Trim(), "Member");

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id:int}/role")]
        [Authorize(Policy = AuthorizationPolicies.OwnerOnly)]
        [ProducesResponseType(typeof(UserRoleChangeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserRoleChangeResponse>> ChangeRole(int id, ChangeUserRoleRequest request, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var result = await _memberRoleManagementService.ChangeRoleAsync(currentUser, id, request.Role!.Value, cancellationToken);

            if (result.Succeeded)
            {
                if (result.UserId is not int userId || result.Role is not UserRole role)
                {
                    throw new InvalidOperationException("A successful role change must return the user ID and role.");
                }

                return Ok(new UserRoleChangeResponse(userId, role.ToString()));
            }

            return result.Failure switch
            {
                UserRoleChangeFailure.CallerNotOwner => Forbid(),

                UserRoleChangeFailure.UserNotFound => NotFound(),

                UserRoleChangeFailure.InvalidRole => BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid role.",
                    Detail = "The requested role is not supported."
                }),

                UserRoleChangeFailure.OwnerRoleCannotBeAssigned => BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid role change.",
                    Detail = "The Owner role cannot be assigned through this operation."
                }),

                UserRoleChangeFailure.OwnerRoleCannotBeChanged => Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Role change conflict.",
                    Detail = "An Owner's role cannot be changed through this operation."
                }),

                _ => throw new InvalidOperationException($"Unexpected role-change failure: {result.Failure}.")
            };
        }

        [HttpPut("{id:int}/team")]
        [Authorize(Policy = AuthorizationPolicies.AdministratorOrOwner)]
        [ProducesResponseType(typeof(TeamMembershipResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamMembershipResponse>> AssignToTeam(int id, AssignUserToTeam request, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var result = await _teamMembershipService.AssignAsync(currentUser, id, request.TeamId!.Value, cancellationToken);

            if (result.Succeeded)
            {
                return Ok(new TeamMembershipResponse(result.UserId!.Value, result.TeamId));
            }

            return result.Failure switch
            {
                TeamMembershipFailure.CallerNotAdministratorOrOwner => Forbid(),

                TeamMembershipFailure.UserNotFound => NotFound(),

                TeamMembershipFailure.TeamNotFound => NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Team not found.",
                    Detail = "The requested team was not found in your organization."
                }),

                _ => throw new InvalidOperationException($"Unexpected team membership failure: {result.Failure}.")
            };
        }

        [HttpDelete("{id:int}/team")]
        [Authorize(Policy = AuthorizationPolicies.AdministratorOrOwner)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromTeam(int id, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var result = await _teamMembershipService.RemoveAsync(currentUser, id, cancellationToken);

            if (result.Succeeded) return NoContent();

            return result.Failure switch
            {
                TeamMembershipFailure.CallerNotAdministratorOrOwner => Forbid(),
                TeamMembershipFailure.UserNotFound => NotFound(),

                _ => throw new InvalidOperationException($"Unexpected team membership failure: {result.Failure}.")
            };
        }

        private async Task<CurrentUserAccessContext?> ResolveCurrentUserAsync(CancellationToken cancellationToken)
        {
            string? identityUserId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId)) return null;

            return await _currentUserContextService.GetByIdentityUserIdAsync(identityUserId, cancellationToken);
        }
    }
}
