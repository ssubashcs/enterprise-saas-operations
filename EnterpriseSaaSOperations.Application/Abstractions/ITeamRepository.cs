using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface ITeamRepository  
    {
        void Add(Team team);

        Task<Team?> GetByIdAsync(int teamId, int organizationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a tracked team for modification within its organization.
        /// </summary>
        Task<Team?> GetByIdForUpdateAsync(int teamId, int organizationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves one page of teams belonging to an organization.
        /// </summary>
        Task<IReadOnlyList<Team>> GetPageAsync(int organizationId, int skip, int take, CancellationToken cancellationToken = default);

        Task<int> CountByOrganizationAsync(int organizationId, CancellationToken cancellationToken = default);
    }
}
