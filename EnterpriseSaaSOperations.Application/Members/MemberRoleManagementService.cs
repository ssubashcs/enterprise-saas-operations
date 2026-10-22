using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.Members
{
    public class MemberRoleManagementService(IDomainUserRepository domainUserRepository, IUnitOfWork unitOfWork)
    {
        private readonly IDomainUserRepository _domainUserRepository = domainUserRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<MemberRoleChangeResult> ChangeRoleAsync(CurrentUserAccessContext currentUser, int targetUserId,
                                                                  UserRole newRole, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(currentUser);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(targetUserId);

            if (currentUser.Role != UserRole.Owner)
            {
                return new MemberRoleChangeResult(false, null, null, MemberRoleChangeFailure.CallerNotOwner);
            }

            if (!Enum.IsDefined(newRole))
            {
                return new MemberRoleChangeResult(false, null, null, MemberRoleChangeFailure.InvalidRole);
            }

            if (newRole == UserRole.Owner)
            {
                return new MemberRoleChangeResult(false, null, null, MemberRoleChangeFailure.OwnerRoleCannotBeAssigned); 
            }

            var targetUser = await _domainUserRepository.GetByIdForUpdateAsync(targetUserId, currentUser.OrganizationId, cancellationToken);

            if (targetUser is null)
            {
                return new MemberRoleChangeResult(false, null, null, MemberRoleChangeFailure.UserNotFound);
            }

            if (targetUser.Role == UserRole.Owner)
            {
                return new MemberRoleChangeResult(false, null, null, MemberRoleChangeFailure.OwnerRoleCannotBeChanged);
            }

            targetUser.ChangeRole(newRole);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new MemberRoleChangeResult(true, targetUser.Id, targetUser.Role, null);
        }
    }   
}
