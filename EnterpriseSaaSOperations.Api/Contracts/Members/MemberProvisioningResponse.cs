namespace EnterpriseSaaSOperations.Api.Contracts.Members
{
    public sealed record MemberProvisioningResponse(int UserId,
                                                    int OrganizationId,
                                                    int? TeamId,
                                                    string Name,
                                                    string Email,
                                                    string Role);
}
