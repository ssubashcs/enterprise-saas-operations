namespace EnterpriseSaaSOperations.Application.Authentication
{
    /// <summary>
    /// Identifies an Identity account after successful credential verification.
    /// </summary>
    public sealed record VerifiedCredentials(string IdentityUserId);
}
