using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    public sealed class CurrentUserContextService(OperationsDbContext dbContext) : ICurrentUserContextService
    {
        private readonly OperationsDbContext _dbContext = dbContext; 

        /// <inheritdoc/>
        public async Task<CurrentUserAccessContext?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identityUserId);

            // Users is Identity's DbSet; DomainUsers represents our business users.
            return await (from identityUser in _dbContext.Users.AsNoTracking()
                          join domainUser in _dbContext.DomainUsers.AsNoTracking()
                            on identityUser.DomainUserId equals domainUser.Id
                          where identityUser.Id == identityUserId
                          select new CurrentUserAccessContext(domainUser.Id,
                                                              domainUser.OrganizationId,
                                                              domainUser.Name,
                                                              domainUser.Email,
                                                              domainUser.Role))
                                                              .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
