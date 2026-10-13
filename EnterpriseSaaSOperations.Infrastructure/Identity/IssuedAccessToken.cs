namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    public sealed record IssuedAccessToken(string Value, 
                                           DateTimeOffset ExpiresAtUtc);
}
