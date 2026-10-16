using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.Organizations
{
    /// <summary>
    /// Coordinates atomic creation of an organization, its Owner, and the Owner's authentication account.
    /// </summary>
    public sealed class OrganizationOnboardingService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IDomainUserRepository _domainUserRepository;
        private readonly IIdentityAccountManager _identityAccountManager;
        private readonly IUnitOfWork _unitOfWork;

        public OrganizationOnboardingService(IOrganizationRepository organizationRepository, IDomainUserRepository domainUserRepository,
                                             IIdentityAccountManager identityAccountManager, IUnitOfWork unitOfWork)
        {
            _organizationRepository = organizationRepository;
            _domainUserRepository = domainUserRepository;
            _identityAccountManager = identityAccountManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrganizationOnboardingResult> CreateOrganizationWithOwnerAsync(string organizationName, string ownerName,
                                                                                         string email, string password, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(organizationName);
            ArgumentException.ThrowIfNullOrWhiteSpace(ownerName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                Organization organization = new(organizationName);

                _organizationRepository.Add(organization);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var emailAddress = email.Trim();

                User owner = new(organization.Id, ownerName, emailAddress, UserRole.Owner);

                _domainUserRepository.Add(owner);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var accountResult = await _identityAccountManager.CreateAsync(owner.Id, owner.Email, password);

                if (!accountResult.Succeeded)
                {
                    await transaction.RollbackAsync(CancellationToken.None);

                    _unitOfWork.ClearTrackedChanges();
                    
                    return new OrganizationOnboardingResult(false, null, null, accountResult.Errors);
                }

                cancellationToken.ThrowIfCancellationRequested();

                await transaction.CommitAsync(CancellationToken.None);

                return new OrganizationOnboardingResult(true, organization.Id, owner.Id, Array.Empty<string>());
            }
            catch
            {
                _unitOfWork.ClearTrackedChanges();

                throw;
            }
        }
    }
}
