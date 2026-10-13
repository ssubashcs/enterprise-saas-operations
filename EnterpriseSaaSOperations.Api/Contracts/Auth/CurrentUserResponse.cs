namespace EnterpriseSaaSOperations.Api.Contracts.Auth
{
    public sealed record CurrentUserResponse(int UserId,
                                             int OrganizationId,
                                             string Name,
                                             string Email,
                                             string Role);
}
