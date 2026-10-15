namespace EnterpriseSaaSOperations.Api.Contracts.Authentication
{
    public sealed record CurrentUserResponse(int UserId,
                                             int OrganizationId,
                                             string Name,
                                             string Email,
                                             string Role);
}
