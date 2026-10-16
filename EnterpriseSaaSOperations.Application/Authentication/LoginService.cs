using EnterpriseSaaSOperations.Application.Abstractions;

namespace EnterpriseSaaSOperations.Application.Authentication
{
    public class LoginService(ICredentialVerifier credentialVerifier, IAccessTokenIssuer accessTokenIssuer)
    {
        private readonly ICredentialVerifier _credentialVerifier = credentialVerifier;
        private readonly IAccessTokenIssuer _accessTokenIssuer = accessTokenIssuer;

        public async Task<IssuedAccessToken?> LoginAsync(string email, string password)
        {
            var credentials = await _credentialVerifier.VerifyCredentialsAsync(email, password);

            if (credentials is null) return null;

            return _accessTokenIssuer.CreateAccessToken(credentials);
        }
    }
}
