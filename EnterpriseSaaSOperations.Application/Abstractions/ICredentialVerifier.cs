using EnterpriseSaaSOperations.Application.Authentication;

namespace EnterpriseSaaSOperations.Application.Abstractions
{
    public interface ICredentialVerifier
    {
        Task<VerifiedCredentials?> VerifyCredentialsAsync(string email, string password);
    }
}
