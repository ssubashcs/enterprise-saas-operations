namespace EnterpriseSaaSOperations.Application.Common
{
    /// <summary>
    /// Contains a page of results and the metadata needed to navigate the result set.
    /// </summary>
    public sealed record PagedResult<T>(IReadOnlyList<T> Items, 
                                        int Page, 
                                        int PageSize, 
                                        int TotalCount);
}
