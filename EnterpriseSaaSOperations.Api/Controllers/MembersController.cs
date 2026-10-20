using EnterpriseSaaSOperations.Api.Authorization;
using EnterpriseSaaSOperations.Api.Contracts.Members;
using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSaaSOperations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly ICurrentUserContextService _currentUserContextService;
        private readonly MemberProvisioningService _memberProvisioningService;

        public MembersController(ICurrentUserContextService currentUserContextService, MemberProvisioningService memberProvisioningService)
        {
            _currentUserContextService = currentUserContextService;
            _memberProvisioningService = memberProvisioningService;
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.AdministratorOrOwner)]
        [ProducesResponseType(typeof(MemberProvisioningResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<MemberProvisioningResponse>> Create(CreateMemberRequest request, CancellationToken cancellationToken)
        {
            var identityUserId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId)) return Unauthorized();

            var currentUser = await _currentUserContextService.GetByIdentityUserIdAsync(identityUserId, cancellationToken);

            if (currentUser is null) return Unauthorized();

            var result = await _memberProvisioningService.CreateAsync(currentUser, request.Name, request.Email, request.Password,
                                                                      request.TeamId, cancellationToken);

            if (!result.Succeeded)
            {
                if (result.Failure == MemberProvisioningFailure.TeamNotFound)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Team not found.",
                        Detail = "The selected team was not found in your organization."
                    });
                }

                // Avoid exposing raw Identity errors, which might reveal
                // whether an email address is already registered.
                var errors = new Dictionary<string, string[]>
                {
                    ["account"] =
                    [
                        "The member account could not be created. " +
                        "Check the email address and password requirements."
                    ]
                };

                return BadRequest(new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Member provisioning failed."
                });
            }

            if (result.MemberUserId is not int memberUserId)
            {
                throw new InvalidOperationException("Successful provisioning must return the member's ID.");
            }

            var response = new MemberProvisioningResponse(memberUserId, currentUser.OrganizationId, result.TeamId,
                                                          request.Name.Trim(), request.Email.Trim(), "Member");

            // Member retrieval is not exposed by this controller yet,
            // so there is no resource URL to provide in a Location header.
            return StatusCode(StatusCodes.Status201Created, response);
        }
    }
}
