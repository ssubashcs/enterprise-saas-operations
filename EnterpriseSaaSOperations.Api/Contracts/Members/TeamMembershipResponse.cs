namespace EnterpriseSaaSOperations.Api.Contracts.Members
{
    public sealed record TeamMembershipResponse(int UserId,
                                                int? TeamId);
}
