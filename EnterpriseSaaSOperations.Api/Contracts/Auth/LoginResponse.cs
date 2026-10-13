namespace EnterpriseSaaSOperations.Api.Contracts.Auth
{
    public sealed record LoginResponse(string AccessToken, 
                                       string TokenType, 
                                       DateTimeOffset ExpiresAtUtc);
}
