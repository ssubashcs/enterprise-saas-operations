using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using EnterpriseSaaSOperations.Application.Abstractions;
using EnterpriseSaaSOperations.Infrastructure.Persistence.Repositories;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            services.AddDbContext<OperationsDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IOperationalRequestRepository, OperationalRequestRepository>()
                    .AddScoped<IOrganizationRepository, OrganizationRepository>()
                    .AddScoped<IDomainUserRepository, DomainUserRepository>()
                    .AddScoped<IUnitOfWork, EfCoreUnitOfWork>()
                    .AddScoped<ITeamRepository, TeamRepository>()
                    .AddScoped<IAuditLogRepository, AuditLogRepository>();

            return services;
        }
    }
}
