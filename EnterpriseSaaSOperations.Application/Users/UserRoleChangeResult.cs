using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.Users
{
    public sealed record UserRoleChangeResult(bool Succeeded,
                                              int? UserId,
                                              UserRole? Role,
                                              UserRoleChangeFailure? Failure);
}
