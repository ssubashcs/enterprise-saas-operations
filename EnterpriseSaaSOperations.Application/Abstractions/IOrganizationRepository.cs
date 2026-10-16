using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface IOrganizationRepository
    {
        /// <summary>
        /// Adds an organization to the current unit of work.
        /// </summary>
        void Add(Organization organization);
    }
}
