using EnterpriseSaaSOperations.Api.Contracts.Authentication;
using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSaaSOperations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly ICurrentUserContextService _currentUserContextService;

        public AuthController(LoginService loginService, ICurrentUserContextService currentUserContextService)
        {
            _loginService = loginService;
            _currentUserContextService = currentUserContextService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var issuedToken = await _loginService.LoginAsync(request.Email, request.Password);

            if (issuedToken is null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Authentication failed.",
                    Detail = "The email or password is incorrect."
                });
            }

            LoginResponse response = new(issuedToken.Value, "Bearer", issuedToken.ExpiresAtUtc);

            return Ok(response);
        }

        /// <summary>
        /// Returns the authenticated user's current domain identity and access context.
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CurrentUserResponse>> GetCurrentUser(CancellationToken cancellationToken)
        {
            // MapInboundClaims is disabled, so the JWT subject remains "sub".
            var identityUserId = User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized();
            }

            var currentUser = await _currentUserContextService.GetByIdentityUserIdAsync(identityUserId,
                                                                                        cancellationToken);

            if (currentUser is null) return Unauthorized();

            CurrentUserResponse response = new(currentUser.UserId,
                                               currentUser.OrganizationId,
                                               currentUser.Name,
                                               currentUser.Email,
                                               currentUser.Role.ToString());

            return Ok(response);
        }
    }
}
