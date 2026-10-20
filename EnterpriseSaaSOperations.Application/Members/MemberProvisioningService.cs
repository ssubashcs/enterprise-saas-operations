using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.Members
{
    /// <summary>
    /// Coordinates member creation and the corresponding Identity account.
    /// </summary>
    public sealed class MemberProvisioningService
    {
        private readonly IDomainUserRepository _domainUserRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IIdentityAccountManager _identityAccountManager;
        private readonly IUnitOfWork _unitOfWork;

        public MemberProvisioningService(IDomainUserRepository domainUserRepository, ITeamRepository teamRepository,
                                         IIdentityAccountManager identityAccountManager, IUnitOfWork unitOfWork)
        {
            _domainUserRepository = domainUserRepository;
            _teamRepository = teamRepository;
            _identityAccountManager = identityAccountManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<MemberProvisioningResult> CreateAsync(CurrentUserAccessContext currentUser, string name, string email,
                                                                string password, int? teamId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            if (teamId is <= 0) throw new ArgumentOutOfRangeException(nameof(teamId));

            var emailAddress = email.Trim();

            User member = new(currentUser.OrganizationId, name, emailAddress, UserRole.Member);

            if (teamId is int selectedTeamId)
            {
                var team = await _teamRepository.GetByIdAsync(selectedTeamId, currentUser.OrganizationId, cancellationToken);

                if (team is null)
                {
                    return new MemberProvisioningResult(false, null, selectedTeamId, MemberProvisioningFailure.TeamNotFound, Array.Empty<string>());
                }

                member.AssignToTeam(selectedTeamId);
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _domainUserRepository.Add(member);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                // Identity account creation does not accept a cancellation token.
                var identityResult = await _identityAccountManager.CreateAsync(member.Id, member.Email, password);

                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync(CancellationToken.None);
                    _unitOfWork.ClearTrackedChanges();

                    return new MemberProvisioningResult(false, null, teamId, MemberProvisioningFailure.IdentityAccountCreationFailed, 
                                                        identityResult.Errors);
                }

                cancellationToken.ThrowIfCancellationRequested();

                await transaction.CommitAsync(CancellationToken.None);

                return new MemberProvisioningResult(true, member.Id, member.TeamId, null, Array.Empty<string>());
            }
            catch
            {
                _unitOfWork.ClearTrackedChanges();
                throw;
            }
        }
    }
}
