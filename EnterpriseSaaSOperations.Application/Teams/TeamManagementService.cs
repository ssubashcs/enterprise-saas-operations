using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Common;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Application.Teams
{
    public sealed class TeamManagementService(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        private readonly ITeamRepository _teamRepository = teamRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Team> CreateAsync(CurrentUserAccessContext currentUser, string name, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            Team team = new(currentUser.OrganizationId, name);

            _teamRepository.Add(team);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return team;
        }

        public Task<Team?> GetByIdAsync(CurrentUserAccessContext currentUser, int teamId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            return _teamRepository.GetByIdAsync(teamId, currentUser.OrganizationId, cancellationToken);
        }

        /// <summary>
        /// Retrieves one page of teams from the current user's organization.
        /// </summary>
        /// <param name="currentUser">The access context of the current user.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of teams per page.</param>
        public async Task<PagedResult<Team>> GetPageAsync(CurrentUserAccessContext currentUser, int page,
                                                          int pageSize, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));

            if (pageSize is < 1 or > 100) throw new ArgumentOutOfRangeException(nameof(pageSize));

            var skip = checked((page - 1) * pageSize);

            var totalCount = await _teamRepository.CountByOrganizationAsync(currentUser.OrganizationId, cancellationToken);

            var teams = await _teamRepository.GetPageAsync(currentUser.OrganizationId, skip, pageSize, cancellationToken);

            return new PagedResult<Team>(teams, page, pageSize, totalCount);
        }

        public async Task<Team?> RenameAsync(CurrentUserAccessContext currentUser, int teamId, string newName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);

            var team = await _teamRepository.GetByIdForUpdateAsync(teamId, currentUser.OrganizationId, cancellationToken);

            if (team is null) return null;

            team.Rename(newName);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return team;
        }
    }
}
