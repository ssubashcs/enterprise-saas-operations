namespace EnterpriseSaaSOperations.Application.Identity
{
    public sealed record IdentityAccountCreationResult(bool Succeeded,
                                                       IReadOnlyList<string> Errors);
}
