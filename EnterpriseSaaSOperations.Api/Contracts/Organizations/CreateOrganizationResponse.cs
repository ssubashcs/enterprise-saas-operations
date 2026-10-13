namespace EnterpriseSaaSOperations.Api.Contracts.Organizations
{
    public sealed record CreateOrganizationResponse(int OrganizationId, 
                                                    int OwnerUserId);
}
