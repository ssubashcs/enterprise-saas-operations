using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;
using EnterpriseSaaSOperations.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    /// <summary>
    /// Coordinates the creation of an organization, its initial domain user, and the corresponding Identity account as one database transaction.
    /// </summary>
    public sealed class OrganizationOnboardingService(OperationsDbContext dbContext, UserManager<ApplicationIdentityUser> userManager)
    {
        private readonly OperationsDbContext _dbContext = dbContext;
        private readonly UserManager<ApplicationIdentityUser> _userManager = userManager;

        public async Task<OrganizationOnboardingResult> CreateOrganizationWithOwnerAsync(string organizationName, string ownerName,
                                                                                         string email, string password, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(organizationName);
            ArgumentException.ThrowIfNullOrWhiteSpace(ownerName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                Organization organization = new(organizationName);

                _dbContext.Organizations.Add(organization);
                await _dbContext.SaveChangesAsync(cancellationToken);

                string emailAddress = email.Trim();

                User owner = new(organization.Id, ownerName, emailAddress, UserRole.Owner);

                _dbContext.DomainUsers.Add(owner);
                await _dbContext.SaveChangesAsync(cancellationToken);

                ApplicationIdentityUser identityUser = new()
                {
                    UserName = emailAddress,
                    Email = emailAddress,
                    DomainUserId = owner.Id
                };

                var identityResult = await _userManager.CreateAsync(identityUser, password);

                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync(CancellationToken.None);

                    _dbContext.ChangeTracker.Clear();

                    var errors = identityResult.Errors.Select(error => error.Description).ToArray();

                    return new OrganizationOnboardingResult(false, null, null, errors);
                }

                cancellationToken.ThrowIfCancellationRequested();

                await transaction.CommitAsync(cancellationToken);

                return new OrganizationOnboardingResult(true, organization.Id, owner.Id, Array.Empty<string>());
            }
            catch
            {
                _dbContext.ChangeTracker.Clear();

                throw;
            }
        }
    }
}
