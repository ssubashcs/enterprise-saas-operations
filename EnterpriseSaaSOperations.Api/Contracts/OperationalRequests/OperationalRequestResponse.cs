namespace EnterpriseSaaSOperations.Api.Contracts.OperationalRequests
{
    public sealed record OperationalRequestResponse(int Id,
                                                    string Title,
                                                    string Description,
                                                    string Status,
                                                    string Priority,
                                                    int? TeamId,
                                                    int? AssigneeUserId,
                                                    DateTimeOffset CreatedAt,
                                                    DateTimeOffset UpdatedAt);
}
