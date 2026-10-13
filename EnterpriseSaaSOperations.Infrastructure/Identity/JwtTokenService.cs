using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    public sealed class JwtTokenService
    {
        private readonly JwtOptions _options;
        private readonly SymmetricSecurityKey _signingKey;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;

            _signingKey = new SymmetricSecurityKey(Convert.FromBase64String(_options.SigningKey));
        }

        public IssuedAccessToken CreateAccessToken(VerifiedCredentials credentials)
        {
            ArgumentNullException.ThrowIfNull(credentials);

            var issuedAt = DateTimeOffset.UtcNow;
            var expiresAt = issuedAt.AddMinutes(_options.AccessTokenLifetimeMinutes);

            Claim[] claims = new[]
            {
                // Identifies the authenticated Identity account.
                new Claim(JwtRegisteredClaimNames.Sub, credentials.IdentityUserId),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),

                new Claim(JwtRegisteredClaimNames.Iat, issuedAt.ToUnixTimeSeconds()
                                                               .ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            JwtSecurityToken token = new(_options.Issuer, _options.Audience, claims, issuedAt.UtcDateTime, 
                                            expiresAt.UtcDateTime, new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256));

            // Serializes a JwtSecurityToken into a JWT in a compact serialization format.                                             
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return new IssuedAccessToken(tokenValue, expiresAt);
        }
    }
}
