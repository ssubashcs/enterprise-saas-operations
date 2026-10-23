using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.OperationalRequests
{
    /// <summary>
    /// Coordinates operational-request creation and retrieval.
    /// </summary>
    public sealed class OperationalRequestService(IOperationalRequestRepository repository, ITeamRepository teamRepository,
                                                  IDomainUserRepository domainUserRepository, IUnitOfWork unitOfWork)
    {
        private readonly IOperationalRequestRepository _repository = repository;
        private readonly ITeamRepository _teamRepository = teamRepository;
        private readonly IDomainUserRepository _domainUserRepository = domainUserRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Creates a request for the authenticated user's organization.
        /// </summary>
        public async Task<OperationalRequest> CreateAsync(CurrentUserAccessContext currentUser, string title, 
                                                          string description, RequestPriority priority,
                                                          CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            OperationalRequest request = new(currentUser.OrganizationId, currentUser.UserId, title,
                                             description, priority);

            await _repository.AddAsync(request, cancellationToken);

            return request;
        }

        /// <summary>
        /// Retrieves an operational request only if it belongs to the specified organization.
        /// </summary>
        public Task<OperationalRequest?> GetByIdAsync(int requestId, int organizationId, CancellationToken cancellationToken = default)
        {
            return _repository.GetByIdAsync(requestId, organizationId, cancellationToken);
        }

        public async Task<RequestAssignmentResult> AssignAsync(CurrentUserAccessContext currentUser, int requestId, int teamId,
                                                               int assigneeUserId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            if (currentUser.Role is not (UserRole.Administrator or UserRole.Owner))
            {
                return Failure(RequestAssignmentFailure.CallerNotAuthorized);
            }

            if (requestId <= 0 || teamId <= 0 || assigneeUserId <= 0)
            {
                return Failure(RequestAssignmentFailure.InvalidAssignment);
            }

            OperationalRequest? request = await _repository.GetByIdForUpdateAsync(requestId, currentUser.OrganizationId, cancellationToken);

            if (request is null) return Failure(RequestAssignmentFailure.RequestNotFound);

            Team? team = await _teamRepository.GetByIdAsync(teamId, currentUser.OrganizationId, cancellationToken);

            if (team is null) return Failure(RequestAssignmentFailure.TeamNotFound);

            User? assignee = await _domainUserRepository.GetByIdAsync(assigneeUserId, currentUser.OrganizationId, cancellationToken);

            if (assignee is null) return Failure(RequestAssignmentFailure.AssigneeNotFound);

            if (assignee.TeamId != team.Id)
            {
                return Failure(RequestAssignmentFailure.AssigneeNotInTeam);
            }

            try
            {
                request.Assign(team.Id, assignee.Id);
            }
            catch (InvalidOperationException)
            {
                return Failure(RequestAssignmentFailure.InvalidRequestStatus);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RequestAssignmentResult(true, request.Id, team.Id, assignee.Id, request.Status, null);
        }

        private static RequestAssignmentResult Failure(RequestAssignmentFailure failure)
        {
            return new RequestAssignmentResult(false, null, null, null, null, failure);
        }
    }
}
