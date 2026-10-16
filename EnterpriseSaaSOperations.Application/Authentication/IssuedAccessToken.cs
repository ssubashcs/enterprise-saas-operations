namespace EnterpriseSaaSOperations.Application.Authentication
{
    public sealed record IssuedAccessToken(string Value, 
                                           DateTimeOffset ExpiresAtUtc);
}
