using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.Teams;

public sealed class TeamMembershipService(IDomainUserRepository domainUserRepository, ITeamRepository teamRepository,
                                          IUnitOfWork unitOfWork)
{
    private readonly IDomainUserRepository _domainUserRepository = domainUserRepository;
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TeamMembershipResult> AssignAsync(CurrentUserAccessContext currentUser, int userId, int teamId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(currentUser);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(teamId);

        if (currentUser.Role is not (UserRole.Administrator or UserRole.Owner))
        {
            return new TeamMembershipResult(false, null, null, TeamMembershipFailure.CallerNotAdministratorOrOwner);
        }

        var user = await _domainUserRepository.GetByIdForUpdateAsync(userId, currentUser.OrganizationId, cancellationToken); 

        if (user is null)
        {
            return new TeamMembershipResult(false, null, null, TeamMembershipFailure.UserNotFound);
        }

        var team = await _teamRepository.GetByIdAsync(teamId, currentUser.OrganizationId, cancellationToken);

        if (team is null)
        {
            return new TeamMembershipResult(false, null, null, TeamMembershipFailure.TeamNotFound);
        }

        user.AssignToTeam(team.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TeamMembershipResult(true, user.Id, user.TeamId, null);
    }

    public async Task<TeamMembershipResult> RemoveAsync(CurrentUserAccessContext currentUser, int userId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(currentUser);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

        if (currentUser.Role is not (UserRole.Administrator or UserRole.Owner))
        {
            return new TeamMembershipResult(false, null, null, TeamMembershipFailure.CallerNotAdministratorOrOwner);
        }

        var user = await _domainUserRepository.GetByIdForUpdateAsync(userId, currentUser.OrganizationId, cancellationToken);

        if (user is null)
        {
            return new TeamMembershipResult(false, null, null, TeamMembershipFailure.UserNotFound);
        }

        user.RemoveFromTeam();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TeamMembershipResult(true, user.Id, null, null);
    }
}
