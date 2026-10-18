using EnterpriseSaaSOperations.Api.Authorization;
using EnterpriseSaaSOperations.Api.Contracts.Common;
using EnterpriseSaaSOperations.Api.Contracts.Teams;
using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Application.Teams;
using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseSaaSOperations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController : ControllerBase
    {
        private readonly ICurrentUserContextService _currentUserContextService;
        private readonly TeamManagementService _teamManagementService;

        public TeamsController(ICurrentUserContextService currentUserContextService, TeamManagementService teamManagementService)
        {
            _currentUserContextService = currentUserContextService;
            _teamManagementService = teamManagementService;
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.AdministratorOrOwner)]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<TeamResponse>> Create(CreateTeamRequest request, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var team = await _teamManagementService.CreateAsync(currentUser, request.Name, cancellationToken);

            var response = ToResponse(team);

            return CreatedAtAction(nameof(GetById), new { id = team.Id }, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var team = await _teamManagementService.GetByIdAsync(currentUser, id, cancellationToken);

            if (team is null) return NotFound();

            return Ok(ToResponse(team));
        }

        /// <summary>
        /// Retrieves a page of teams belonging to the current user's organization.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<TeamResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResponse<TeamResponse>>> GetPage([FromQuery, Range(1, 1_000_000)] int page = 1,
                                                                            [FromQuery, Range(1, 100)] int pageSize = 20, 
                                                                            CancellationToken cancellationToken = default)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var result = await _teamManagementService.GetPageAsync(currentUser, page, pageSize, cancellationToken);

            var response = new PagedResponse<TeamResponse>(result.Items.Select(ToResponse).ToArray(), result.Page,
                                                           result.PageSize, result.TotalCount);

            return Ok(response);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AuthorizationPolicies.AdministratorOrOwner)]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamResponse>> Rename(int id, RenameTeamRequest request, CancellationToken cancellationToken)
        {
            var currentUser = await ResolveCurrentUserAsync(cancellationToken);

            if (currentUser is null) return Unauthorized();

            var team = await _teamManagementService.RenameAsync(currentUser, id, request.Name, cancellationToken);

            if (team is null) return NotFound();

            return Ok(ToResponse(team));
        }

        private async Task<CurrentUserAccessContext?> ResolveCurrentUserAsync(CancellationToken cancellationToken)
        {
            var identityUserId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId)) return null;

            return await _currentUserContextService.GetByIdentityUserIdAsync(identityUserId, cancellationToken);
        }

        private static TeamResponse ToResponse(Team team)
        {
            return new TeamResponse(team.Id, team.OrganizationId, team.Name, team.CreatedAt);
        }
    }
}
