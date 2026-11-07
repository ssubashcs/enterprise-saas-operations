namespace EnterpriseSaaSOperations.Api.Contracts.Common
{
    /// <summary>
    /// Represents a paginated collection returned by the API.
    /// </summary>
    public sealed record PagedResponse<T>(IReadOnlyList<T> Items,
                                          int Page,
                                          int PageSize,
                                          int TotalCount);
}
