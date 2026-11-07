namespace EnterpriseSaaSOperations.Api.Contracts.Users
{
    public sealed record TeamMembershipResponse(int UserId,
                                                int? TeamId);
}
