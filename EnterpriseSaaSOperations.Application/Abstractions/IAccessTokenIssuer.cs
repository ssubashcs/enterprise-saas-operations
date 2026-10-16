using EnterpriseSaaSOperations.Application.Authentication;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface IAccessTokenIssuer
    {
        IssuedAccessToken CreateAccessToken(VerifiedCredentials credentials);
    }
}
