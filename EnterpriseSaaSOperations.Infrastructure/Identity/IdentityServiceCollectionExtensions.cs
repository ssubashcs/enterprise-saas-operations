using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    /// <summary>
    /// Registers ASP.NET Core Identity and its EF Core persistence store.
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<ApplicationIdentityUser>(options => options.User.RequireUniqueEmail = true)
                    .AddEntityFrameworkStores<OperationsDbContext>()
                    .AddSignInManager();

            // Binds configuration values to JwtOptions properties
            services.AddOptions<JwtOptions>().Bind(configuration.GetRequiredSection(JwtOptions.SectionName))
                                             .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "Jwt:Issuer is required")
                                             .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "Jwt:Audience is required")
                                             .Validate(options => IsValidSigningKey(options.SigningKey), "Jwt:SigningKey must be Base64-encoded and contain at least 32 bytes.")
                                             .Validate(options => options.AccessTokenLifetimeMinutes is >= 1 and <= 60, "The access-token lifetime must be between 1 and 60 minutes.")
                                             .ValidateOnStart();

            // API and authorization handler depend on the Application contract. Infrastructure supplies its implementation.
            services.AddScoped<ICredentialVerifier, AuthenticationService>()
                    .AddScoped<IAccessTokenIssuer, JwtTokenService>()
                    .AddScoped<IIdentityAccountManager, IdentityAccountManager>()
                    .AddScoped<ICurrentUserContextService, CurrentUserContextService>();

            return services;
        }

        private static bool IsValidSigningKey(string signingKey)
        {
            if (string.IsNullOrWhiteSpace(signingKey)) return false;

            try
            {
                return Convert.FromBase64String(signingKey).Length >= 32;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
