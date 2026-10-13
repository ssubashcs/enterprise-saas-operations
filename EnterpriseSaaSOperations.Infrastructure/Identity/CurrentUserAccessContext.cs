using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    public sealed record CurrentUserAccessContext(int UserId,
                                                  int OrganizationId,
                                                  string Name,
                                                  string Email,
                                                  UserRole Role);
}
