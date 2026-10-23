using EnterpriseSaaSOperations.Api.Authorization;
using EnterpriseSaaSOperations.Api.Contracts.OperationalRequests;
using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Application.OperationalRequests;
using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSaaSOperations.Api.Controllers
{
    [Route("api/requests")]
    [ApiController]
    [Authorize]
    public sealed class OperationalRequestsController : ControllerBase
    {
        private readonly ICurrentUserContextService _currentUserContextService;
        private readonly OperationalRequestService _requestService;

        public OperationalRequestsController(ICurrentUserContextService currentUserContextService, OperationalRequestService requestService)
        {
            _currentUserContextService = currentUserContextService;
            _requestService = requestService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationalRequestResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OperationalRequestResponse>> Create(OperationalRequestCreationRequest request, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var createdRequest = await _requestService.CreateAsync(currentUser, request.Title, request.Description,
                                                                   request.Priority!.Value, cancellationToken);

            var response = ToResponse(createdRequest);

            return CreatedAtAction(nameof(GetById), new { id = createdRequest.Id }, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OperationalRequestResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OperationalRequestResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var request = await _requestService.GetByIdAsync(id, currentUser.OrganizationId, cancellationToken);

            if (request is null) return NotFound();

            return Ok(ToResponse(request));
        }

        [HttpPut("{id:int}/assign")]
        [Authorize(Policy = AuthorizationPolicies.AdministratorOrOwner)]
        [ProducesResponseType(typeof(OperationalRequestAssignmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Assign(int id, OperationalRequestAssignmentRequest request, CancellationToken cancellationToken)
        {
            string? identityUserId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId)) return Unauthorized();

            CurrentUserAccessContext? currentUser = await _currentUserContextService.GetByIdentityUserIdAsync(identityUserId, cancellationToken);

            if (currentUser is null) return Unauthorized();

            RequestAssignmentResult result = await _requestService.AssignAsync(currentUser, id, request.TeamId, request.AssigneeUserId, cancellationToken);

            if (result.Success)
            {
                OperationalRequestAssignmentResponse response = new(result.RequestId!.Value, result.TeamId!.Value, 
                                                                    result.AssigneeUserId!.Value, result.Status!.Value);

                return Ok(response);
            }

            return result.Failure switch
            {
                RequestAssignmentFailure.CallerNotAuthorized => Forbid(),

                RequestAssignmentFailure.RequestNotFound or
                RequestAssignmentFailure.TeamNotFound or
                RequestAssignmentFailure.AssigneeNotFound => NotFound(),

                RequestAssignmentFailure.AssigneeNotInTeam => BadRequest(Problem(title: "Invalid assignment",
                                                                                 detail: "The selected assignee must belong to the selected team.",
                                                                                 statusCode: StatusCodes.Status400BadRequest)),

                RequestAssignmentFailure.InvalidAssignment => BadRequest(Problem(title: "Invalid assignment",
                                                                                 detail: "A valid request, team, and assignee must be specified.",
                                                                                 statusCode: StatusCodes.Status400BadRequest)),

                RequestAssignmentFailure.InvalidRequestStatus => Conflict(Problem(title: "Request cannot be assigned",
                                                                                  detail: "The request's current status does not permit assignment.",
                                                                                  statusCode: StatusCodes.Status409Conflict)),

                _ => Problem(statusCode: StatusCodes.Status500InternalServerError,
                             title: "An unexpected error occurred")
            };
        }

        private async Task<CurrentUserAccessContext?> ResolveCurrentUserAsync(CancellationToken cancellationToken)
        {
            var identityUserId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId)) return null;

            return await _currentUserContextService.GetByIdentityUserIdAsync(identityUserId, cancellationToken);
        }

        private static OperationalRequestResponse ToResponse(OperationalRequest request)
        {
            return new OperationalRequestResponse(request.Id,
                                                  request.Title,
                                                  request.Description,
                                                  request.Status.ToString(),
                                                  request.Priority.ToString(),
                                                  request.TeamId,
                                                  request.AssigneeUserId,
                                                  request.CreatedAt,
                                                  request.UpdatedAt);
        }
    }
}
