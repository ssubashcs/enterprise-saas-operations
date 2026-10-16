using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Application.Identity
{
    public sealed record CurrentUserAccessContext(int UserId,
                                                  int OrganizationId,
                                                  string Name,
                                                  string Email,
                                                  UserRole Role);
}
