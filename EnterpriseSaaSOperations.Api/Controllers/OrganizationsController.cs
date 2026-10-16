using EnterpriseSaaSOperations.Api.Contracts.Organizations;
using EnterpriseSaaSOperations.Application.Organizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseSaaSOperations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class OrganizationsController(OrganizationOnboardingService onboardingService) : ControllerBase
    {
        private readonly OrganizationOnboardingService _onboardingService = onboardingService;

        /// <summary>
        /// Creates an organization and its initial Owner account.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateOrganizationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateOrganizationResponse>> Create([FromBody] CreateOrganizationRequest request, CancellationToken cancellationToken)
        {
            OrganizationOnboardingResult result = await _onboardingService.CreateOrganizationWithOwnerAsync(request.OrganizationName,
                                                                                                            request.OwnerName,
                                                                                                            request.Email,
                                                                                                            request.Password,
                                                                                                            cancellationToken);

            if (!result.Succeeded)
            {
                Dictionary<string, string[]> errors = new() 
                {
                    ["account"] = result.Errors.ToArray()
                };

                ValidationProblemDetails problem = new(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Organization onboarding failed."
                };

                return BadRequest(problem);
            }

            if (result.OrganizationId is not int organizationId || result.DomainUserId is not int ownerUserId)
            {
                throw new InvalidOperationException("Successful onboarding must return the generated identifiers.");
            }

            CreateOrganizationResponse response = new(organizationId, ownerUserId);

            return StatusCode(StatusCodes.Status201Created, response);
        }
    }
}
