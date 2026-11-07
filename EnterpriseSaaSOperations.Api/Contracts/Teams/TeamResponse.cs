namespace EnterpriseSaaSOperations.Api.Contracts.Teams
{
    public sealed record TeamResponse(int Id,
                                      int OrganizationId,
                                      string Name,
                                      DateTimeOffset CreatedAt);
}
