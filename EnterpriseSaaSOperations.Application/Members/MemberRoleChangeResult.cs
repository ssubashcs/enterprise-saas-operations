using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.Contracts.Members
{
    public sealed record MemberRoleChangeResult(bool Succeeded,
                                                int? UserId,
                                                UserRole? Role,
                                                MemberRoleChangeFailure? Failure);
}
