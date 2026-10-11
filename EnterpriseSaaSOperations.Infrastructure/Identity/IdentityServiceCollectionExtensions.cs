using EnterpriseSaaSOperations.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    /// <summary>
    /// Registers ASP.NET Core Identity and its EF Core persistence store.
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<ApplicationIdentityUser>(options => options.User.RequireUniqueEmail = true)
                    .AddEntityFrameworkStores<OperationsDbContext>();

            services.AddScoped<OrganizationOnboardingService>();

            return services;
        }
    }
}
