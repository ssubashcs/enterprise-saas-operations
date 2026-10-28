using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Application.Auditing;
using EnterpriseSaaSOperations.Application.Identity;
using EnterpriseSaaSOperations.Domain.Enums;
using System.Text.Json;

namespace EnterpriseSaaSOperations.Application.Members
{
    public class MemberRoleManagementService(IDomainUserRepository domainUserRepository, IUnitOfWork unitOfWork,
                                             IAuditLogRepository auditLogRepository)
    {
        private readonly IDomainUserRepository _domainUserRepository = domainUserRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;

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

            UserRole previousRole = targetUser.Role;

            targetUser.ChangeRole(newRole);

            if (previousRole != targetUser.Role)
            {
                string detailsJson = JsonSerializer.Serialize(new
                {
                    previousRole = previousRole.ToString(),
                    newRole = targetUser.Role.ToString()
                });

                AuditEntry auditEntry = new(currentUser.OrganizationId, currentUser.UserId, AuditAction.MemberRoleChanged,
                                            AuditTargetType.Member, targetUser.Id, DateTimeOffset.UtcNow, detailsJson);

                _auditLogRepository.Add(auditEntry);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new MemberRoleChangeResult(true, targetUser.Id, targetUser.Role, null);
        }
    }   
}
