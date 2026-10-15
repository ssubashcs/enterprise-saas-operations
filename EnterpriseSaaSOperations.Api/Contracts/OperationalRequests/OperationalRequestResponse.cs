namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed record OperationalRequestResponse(int Id,
                                                    string Title,
                                                    string Description,
                                                    string Status,
                                                    string Priority,
                                                    DateTimeOffset CreatedAt,
                                                    DateTimeOffset UpdatedAt);
}
