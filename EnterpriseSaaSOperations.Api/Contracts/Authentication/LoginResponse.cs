namespace EnterpriseSaaSOperations.Api.Contracts.Authentication
{
    public sealed record LoginResponse(string AccessToken, 
                                       string TokenType, 
                                       DateTimeOffset ExpiresAtUtc);
}
