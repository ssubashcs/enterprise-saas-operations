namespace EnterpriseSaaSOperations.Api.Contracts.Users
{
    public sealed record CreateUserResponse(int UserId,
                                            int OrganizationId,
                                            int? TeamId,
                                            string Name,
                                            string Email,
                                            string Role);
}
