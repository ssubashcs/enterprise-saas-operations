using EnterpriseSaaSOperations.Api.Contracts.OperationalRequests;
using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Infrastructure.Identity;
using EnterpriseSaaSOperations.Infrastructure.OperationalRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSaaSOperations.Api.Controllers
{
    [Route("api/requests")]
    [ApiController]
    [Authorize]
    public sealed class OperationalRequestsController : ControllerBase
    {
        private readonly CurrentUserContextService _currentUserContextService;
        private readonly OperationalRequestService _requestService;

        public OperationalRequestsController(CurrentUserContextService currentUserContextService, OperationalRequestService requestService)
        {
            _currentUserContextService = currentUserContextService;
            _requestService = requestService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OperationalRequestResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OperationalRequestResponse>> Create(CreateOperationalRequestRequest request, CancellationToken cancellationToken)
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
                                                  request.CreatedAt,
                                                  request.UpdatedAt);
        }
    }
}
