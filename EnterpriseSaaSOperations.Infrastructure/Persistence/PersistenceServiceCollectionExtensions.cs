using EnterpriseSaaSOperations.Application;
using EnterpriseSaaSOperations.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            services.AddDbContext<OperationsDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IOperationalRequestRepository, OperationalRequestRepository>()
                    .AddScoped<IOrganizationRepository, OrganizationRepository>()
                    .AddScoped<IDomainUserRepository, DomainUserRepository>()
                    .AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

            return services;
        }
    }
}
